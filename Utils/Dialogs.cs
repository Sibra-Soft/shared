namespace Shared.Utils
{
    public static class Dialogs
    {
        private static Dictionary<string, object>? _requestParams;
        private static Dictionary<string, object>? _responseParams;
        private static Types _type = Types.Default;

        public enum Types
        {
            Default,
            Edit
        }

        /// <summary>
        /// Clear all request parameters
        /// </summary>
        public static void ClearRequest()
        {
            _type = Types.Default;
            _requestParams = [];
        }

        /// <summary>
        /// Clear all response parameters
        /// </summary>
        public static void ClearResponse()
        {
            _responseParams = [];
        }

        /// <summary>
        /// Add a new RequestParameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <exception cref="ArgumentNullException">When the RequestParameters object is NULL</exception>
        public static void AddRequestParameter(string name, object value)
        {
            // Check if parameters is NULL
            if (_requestParams == null)
            {
                throw new ArgumentNullException("You must call the `ClearRequest` function first");
            }

            _requestParams.Add(name, value);
        }

        /// <summary>
        /// Add a new ResponseParameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <exception cref="ArgumentNullException">When the ResponseParameters object is NULL</exception>
        public static void AddResponseParameter(string name, object value)
        {
            // Check if parameters is NULL
            if (_responseParams == null)
            {
                throw new ArgumentNullException("You must call the `ClearResponse` function first");
            }

            _responseParams.Add(name, value);
        }

        /// <summary>
        /// Get a response parameter by it's name
        /// </summary>
        /// <typeparam name="T">The return type of the parameter</typeparam>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The value of the parameter using the specified type</returns>
        /// <exception cref="ArgumentNullException">When the ResponseParameters object is NULL</exception>
        /// <exception cref="InvalidOperationException">When a unsupported type is specified</exception>
        public static T GetResponseParameter<T>(string name)
        {
            object result;

            // Check if parameters is NULL
            if (_responseParams == null)
            {
                throw new ArgumentNullException("You must call the `ClearResponse` function first");
            }
            else
            {
                result = _responseParams[name];
            }

            // Convert to type
            if (typeof(T) == typeof(string))
            {
                return (T)(object)result.ToString();
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(result);
            }
            else if (typeof(T) == typeof(List<>))
            {
                return (T)(object)result;
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)result;
            }
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                return (T)result;
            }
            else if (typeof(T) == typeof(Guid))
            {
                return (T)(object)Guid.Parse(result.ToString());
            }

            throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
        }

        /// <summary>
        /// Get a request parameter by it's name
        /// </summary>
        /// <typeparam name="T">The return type of the parameter</typeparam>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The value of the parameter using the specified type</returns>
        /// <exception cref="ArgumentNullException">When the RequestParameter object is NULL</exception>
        /// <exception cref="InvalidOperationException">When a unsupported type is specified</exception>
        public static T GetRequestParameter<T>(string name)
        {
            object result;

            // Check if parameters is NULL
            if (_requestParams == null)
            {
                throw new ArgumentNullException("You must call the `ClearRequest` function first");
            }
            else
            {
                result = _requestParams[name];
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)result.ToString();
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(result);
            }
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                return (T)result;
            }
            else if (typeof(T).IsGenericType &&
                     typeof(T).GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return (T)result;
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)result;
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(result);
            }
            else if (typeof(T) == typeof(Guid))
            {
                return (T)(object)Guid.Parse(result.ToString());
            }

            throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
        }


        /// <summary>
        /// Set the dialog type
        /// </summary>
        /// <param name="dialogType">The dialog type to use</param>
        public static void SetDialogType(Types type)
        {
            _type = type;
        }

        /// <summary>
        /// Get the dialog type
        /// </summary>
        /// <returns>The dialog type</returns>
        public static Types GetDialogType()
        {
            return _type;
        }

        /// <summary>
        /// Shows a dialog without waiting for a result
        /// </summary>
        /// <param name="dialog">FormControl to use and show</param>
        /// <param name="title">The title/caption to use for the dialog</param>
        public static DialogResult Show(Form dialog, string title)
        {
            dialog.Text = title;
            dialog.Show();

            return dialog.DialogResult;
        }

        /// <summary>
        /// Shows a dialog and waits for a result to be defined
        /// </summary>
        /// <param name="dialog">FormControl to use and show</param>
        /// <param name="title">The title/caption to use for the dialog</param>
        /// <param name="parent">The parent form</param>
        /// <returns>The DialogResult of the shown form</returns>
        public static DialogResult ShowAndWait(Form dialog, string title, Form parent)
        {
            dialog.Text = title;
            dialog.ShowDialog(parent);

            return dialog.DialogResult;
        }
    }
}
