using System;

namespace Engage.Automation.Utilities
{
    internal class DocumentClient
    {
        private Uri uri;
        private string v;

        public DocumentClient(Uri uri, string v)
        {
            this.uri = uri;
            this.v = v;
        }
    }
}