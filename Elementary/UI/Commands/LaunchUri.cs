/*
  The MIT License (MIT)
  Copyright © 2016 Steve Guidetti

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the “Software”), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
*/
using System;
using System.Windows.Input;
using Windows.System;

namespace Elementary.UI.Commands
{
    /// <summary>
    /// Command for launching an external Uri.
    /// </summary>
    public class LaunchUri : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Checks whether the parameter is a Uri.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>Whether the command can execute.</returns>
        public bool CanExecute(object parameter)
        {
            return parameter is Uri;
        }

        /// <summary>
        /// Launches the specified Uri.
        /// </summary>
        /// <param name="parameter">The Uri to launch.</param>
        public async void Execute(object parameter)
        {
            await Launcher.LaunchUriAsync(parameter as Uri);
        }
    }
}
