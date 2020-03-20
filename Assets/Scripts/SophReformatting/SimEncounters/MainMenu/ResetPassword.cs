using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ResetPassword
    {
        protected IWebAddress WebAddress { get; }
        protected MessageHandler MessageHandler { get; }
        protected ServerDataReader<string> ServerDataReader { get; }
        public ResetPassword(IWebAddress webAddress, MessageHandler messageHandler)
        {
            WebAddress = webAddress;
            MessageHandler = messageHandler;
            var statusesParser = new StringParser();
            ServerDataReader = new ServerDataReader<string>(statusesParser);
        }

        private const string phpFile = "Login.php";
        private const string actionVariable = "ACTION";
        private const string resetAction = "forgotPassword";
        private const string emailVariable = "email";
        private const string usernameVariable = "username";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Reset(string email, string username)
        {
            var url = WebAddress.GetUrl(phpFile);
            var form = CreateForm(email, username);

            var webRequest = UnityWebRequest.Post(url, form);
            ServerDataReader.Completed += Completed;
            ServerDataReader.Begin(webRequest);
        }

        public WWWForm CreateForm(string email, string username)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, resetAction);

            form.AddField(emailVariable, email);
            form.AddField(usernameVariable, username);

            return form;
        }

        private const string errorSuffix = "--Could not send email";
        private void Completed(object sender, ServerResult<string> e)
        {
            if (e.Outcome != ServerOutcome.Success) {
                MessageHandler.ShowMessage(e.Message, true);
                return;
            }
            if (!e.Result.EndsWith(errorSuffix)) {
                MessageHandler.ShowMessage("Success. Please check email for verification", false);
                return;
            }

            var error = e.Result.Substring(0, e.Result.Length - errorSuffix.Length);
            MessageHandler.ShowMessage("Unable to send email", true);
        }
    }
}
