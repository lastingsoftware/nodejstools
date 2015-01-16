//*********************************************************//
//    Copyright (c) Microsoft. All rights reserved.
//    
//    Apache 2.0 License
//    
//    You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//    
//    Unless required by applicable law or agreed to in writing, software 
//    distributed under the License is distributed on an "AS IS" BASIS, 
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
//    implied. See the License for the specific language governing 
//    permissions and limitations under the License.
//
//*********************************************************//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Microsoft.NodejsTools.Debugger {
    class NodeModule {
        private readonly ReadOnlyCollection<string> _fileNames;
        private readonly int _id;
        private readonly string _javaScriptFileName;
        private readonly bool _isMappedModule;
        private Dictionary<string, object> _documents;

        public NodeModule(int id, string fileName) : this(id, new[] { fileName }, fileName) {
        }

        public NodeModule(int id, string[] fileNames, string javaScriptFileName) {
            Debug.Assert(javaScriptFileName != null);
            Debug.Assert(fileNames.Length > 0);

            _id = id;
            _fileNames = new ReadOnlyCollection<string>(fileNames);
            _javaScriptFileName = javaScriptFileName;

            this._documents = new Dictionary<string, object>();
            this._isMappedModule = fileNames.Length > 1 || fileNames[0] != javaScriptFileName;
        }

        public int Id {
            get { return _id; }
        }

        public string Name {
            get {
                if (_javaScriptFileName.IndexOfAny(Path.GetInvalidPathChars()) == -1) {
                    return Path.GetFileName(_javaScriptFileName);
                }
                return _javaScriptFileName;
            }
        }

        public string JavaScriptFileName {
            get { return _javaScriptFileName; }
        }

        public ReadOnlyCollection<string> FileNames {
            get { return _fileNames; }
        }

        public bool IsMappedModule { 
            get { return this._isMappedModule; } 
        }

        public string Source { get; set; }

        public bool BuiltIn {
            get {
                // No directory separator characters implies builtin
                return (_javaScriptFileName.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) == -1);
            }
        }

        public IDictionary<string, object> Documents {
            get {
                return this._documents;
            }
        }
    }
}