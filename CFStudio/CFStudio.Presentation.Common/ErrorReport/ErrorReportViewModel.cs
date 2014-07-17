#region License

// --------------------------------------------------------------------------- 
// <copyright file="ErrorReportViewModel.cs" company="Kris Janssen">
//   Copyright (c) 2014-2014 Kris Janssen
// </copyright>
//  This file is part of CFStudio.
//  CFStudio is an open source tool foor confocal microscopy.
//  Parts of the CFStudio codebase were re-used, with permission from
//  the SambaPOS project <https://github.com/emreeren/SambaPOS-3>.
//  CFStudio is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// --------------------------------------------------------------------------- 
#endregion

namespace CFStudio.Presentation.Common.ErrorReport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Windows;

    using CFStudio.Infrastructure.ExceptionReporter;
    using CFStudio.Presentation.Common.Commands;

    using Microsoft.Win32;

    /// <summary>
    ///     The error report view model.
    /// </summary>
    internal class ErrorReportViewModel : ObservableObject
    {
        #region Fields

        /// <summary>
        ///     The _dialog result.
        /// </summary>
        private bool? _dialogResult;

        /// <summary>
        ///     The _error report as text.
        /// </summary>
        private string _errorReportAsText;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReportViewModel"/> class.
        /// </summary>
        /// <param name="exceptions">
        /// The exceptions.
        /// </param>
        public ErrorReportViewModel(IEnumerable<Exception> exceptions)
        {
            this.Model = new ExceptionReportInfo { AppAssembly = Assembly.GetCallingAssembly() };
            this.Model.SetExceptions(exceptions);

            this.CopyCommand = new CaptionCommand<string>("Copy", this.OnCopyCommand);
            this.SaveCommand = new CaptionCommand<string>("Save", this.OnSaveCommand);
            this.SubmitCommand = new CaptionCommand<string>("Send", this.OnSubmitCommand);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the close command.
        /// </summary>
        public ICaptionCommand CloseCommand { get; set; }

        /// <summary>
        ///     Gets or sets the copy command.
        /// </summary>
        public ICaptionCommand CopyCommand { get; set; }

        /// <summary>
        ///     Gets or sets the dialog result.
        /// </summary>
        public bool? DialogResult
        {
            get
            {
                return this._dialogResult;
            }

            set
            {
                this._dialogResult = value;
                this.RaisePropertyChanged(() => this.DialogResult);
            }
        }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this.Model.MainException.Message;
            }
        }

        /// <summary>
        ///     Gets or sets the error report as text.
        /// </summary>
        public string ErrorReportAsText
        {
            get
            {
                return this._errorReportAsText ?? (this._errorReportAsText = this.GenerateReport());
            }

            set
            {
                this._errorReportAsText = value;
            }
        }

        /// <summary>
        ///     Gets or sets the model.
        /// </summary>
        public ExceptionReportInfo Model { get; set; }

        /// <summary>
        ///     Gets or sets the save command.
        /// </summary>
        public ICaptionCommand SaveCommand { get; set; }

        /// <summary>
        ///     Gets or sets the submit command.
        /// </summary>
        public ICaptionCommand SubmitCommand { get; set; }

        /// <summary>
        ///     Gets or sets the user message.
        /// </summary>
        public string UserMessage
        {
            get
            {
                return this.Model.UserExplanation;
            }

            set
            {
                this.Model.UserExplanation = value;
                this._errorReportAsText = null;
                this.RaisePropertyChanged(() => this.ErrorReportAsText);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The get error report.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string GetErrorReport()
        {
            this._errorReportAsText = null;
            return this.ErrorReportAsText;
        }

        /// <summary>
        /// The save report to file.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public void SaveReportToFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            try
            {
                using (FileStream stream = File.OpenWrite(fileName))
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(this.ErrorReportAsText);
                    writer.Flush();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("Unable to save file '{0}' : {1}", fileName, exception.Message));
            }
        }

        /// <summary>
        ///     The submit error.
        /// </summary>
        public void SubmitError()
        {
            string tempFile = Path.GetTempFileName().Replace(".tmp", ".txt");
            this.SaveReportToFile(tempFile);
            string queryString = string.Format(
                "from={0}&emaila={1}&file={2}", 
                Uri.EscapeDataString("info@sambapos.com"), 
                Uri.EscapeDataString("CFStudio Error Report"), 
                Uri.EscapeDataString(tempFile));

            var c = new WebClient();
            byte[] result = c.UploadFile("http://reports.sambapos.com/file.php?" + queryString, "POST", tempFile);
            MessageBox.Show(Encoding.ASCII.GetString(result));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The generate report.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        private string GenerateReport()
        {
            var rg = new ExceptionReportGenerator(this.Model);
            return rg.CreateExceptionReport();
        }

        /// <summary>
        /// The on copy command.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        private void OnCopyCommand(object obj)
        {
            Clipboard.SetText(this.ErrorReportAsText);
        }

        /// <summary>
        /// The on save command.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        private void OnSaveCommand(string obj)
        {
            var sf = new SaveFileDialog();
            string fn = sf.FileName;
            if (!string.IsNullOrEmpty(fn))
            {
                this.SaveReportToFile(fn);
            }
        }

        /// <summary>
        /// The on submit command.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        private void OnSubmitCommand(string obj)
        {
            if (string.IsNullOrEmpty(this.UserMessage))
            {
                if (
                    MessageBox.Show(
                        "You are sending error report without your feedback. "
                        + "Please provide some details about how it happend." + "\r\nContinue without adding details?", 
                        "Information", 
                        MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            this.DialogResult = false;
            this.SubmitError();
        }

        #endregion
    }
}