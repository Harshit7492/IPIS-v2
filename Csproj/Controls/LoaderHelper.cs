using System;
using System.Threading.Tasks;
using System.Windows.Forms;        // For Form, Control, MessageBox
using System.Drawing;              // For Point
using IPISv2.Controls;            // For LoaderControl

namespace Helpers
{
    // Manages the loader instance
    public static class LoaderManager
    {
        private static LoaderControl loader;
        private static int loadingCounter = 0;
        private static Form parentForm;

        public static void Initialize(Control parent)
        {
            if (loader != null) return;

            loader = new LoaderControl();

            // Find the parent form
            parentForm = parent.FindForm();
            if (parentForm != null)
            {
                // Position the loader in the center
                loader.Location = new Point(
                    (parentForm.ClientSize.Width - loader.Width) / 2,
                    (parentForm.ClientSize.Height - loader.Height) / 2
                );

                // Add to form
                parentForm.Controls.Add(loader);
                loader.BringToFront();

                // Handle form resize to keep loader centered
                parentForm.Resize += (s, e) =>
                {
                    if (loader != null && loader.Visible)
                    {
                        loader.Location = new Point(
                            (parentForm.ClientSize.Width - loader.Width) / 2,
                            (parentForm.ClientSize.Height - loader.Height) / 2
                        );
                    }
                };

                // Handle form closing to clean up
                parentForm.FormClosing += (s, e) =>
                {
                    loader?.StopLoading();
                };
            }
        }

        public static void Show()
        {
            if (loader == null)
            {
                // Try to find the loader on the parent form
                if (parentForm != null)
                {
                    foreach (Control ctrl in parentForm.Controls)
                    {
                        if (ctrl is LoaderControl control)
                        {
                            loader = control;
                            break;
                        }
                    }
                }

                if (loader == null) return;
            }

            loadingCounter++;
            if (loadingCounter == 1)
            {
                loader.StartLoading();
            }
        }

        public static void Hide()
        {
            if (loader == null) return;

            loadingCounter--;
            if (loadingCounter <= 0)
            {
                loadingCounter = 0;
                loader.StopLoading();
            }
        }

        public static void Reset()
        {
            loadingCounter = 0;
            loader?.StopLoading();
        }
    }

    // Async helper with loader support
    public static class AsyncHelper
    {
        public static async Task Execute(Func<Task> action, string errorMessage = "Operation failed")
        {
            try
            {
                LoaderManager.Show();
                await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{errorMessage}: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                LoaderManager.Hide();
            }
        }

        public static async Task<T> Execute<T>(Func<Task<T>> action, string errorMessage = "Operation failed")
        {
            try
            {
                LoaderManager.Show();
                return await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{errorMessage}: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return default(T);
            }
            finally
            {
                LoaderManager.Hide();
            }
        }
    }
}