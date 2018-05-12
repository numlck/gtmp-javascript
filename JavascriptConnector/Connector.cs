using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Noesis.Javascript;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.IO;
using System.Reflection;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server.Util;
using GrandTheftMultiplayer;
using System.Diagnostics;

namespace JavascriptConnector
{
    public class Connector : Script
    {
        //public static Dictionary<string, ScriptObject> Scripts = new Dictionary<string, ScriptObject>();
        public V8ScriptEngine engine = new V8ScriptEngine();
        public Connector()
        {
            API.onResourceStart += Init;

        }
        public ScriptObject Import(string package)
        {
            string package_name = package.Replace(".js", "");
            package_name = package_name.Replace("/", "_");
            if (!(this.engine.Script.modules[package_name] is Microsoft.ClearScript.Undefined))
            {
                API.consoleOutput("'" + package + "' Already Exists");
                return this.engine.Script.modules[package_name];
            }

            string js = File.ReadAllText("resources/JavascriptConnector/" + package);
            API.consoleOutput("Loading " + package);
            try
            {
                this.engine.Execute($@"modules['{package_name}'] = (function(exports){{
                    {js}
                    return exports;
                }})({{}})");
            }
            catch (Exception z)
            {
                API.consoleOutput(z.ToString());
            }
            //Connector.Scripts.Add(package, e.Script);
            //connector.API.consoleOutput(this.connector.e.Script.modulesLoaded[package_name]);
            return this.engine.Script.modules[package_name];
        }
        public V8ScriptEngine ExposeAPI(V8ScriptEngine engine)
        {
            var connector = this;
            engine.AddHostObject("API", HostItemFlags.DirectAccess, API);
            engine.AddHostObject("host", new ExtendedHostFunctions());
            engine.AddHostObject("lib", new HostTypeCollection("mscorlib", "System.IO", "MySql.Data"));


            engine.AddHostType("Vector3", typeof(GrandTheftMultiplayer.Shared.Math.Vector3));
            engine.AddHostType("Quaternion", typeof(GrandTheftMultiplayer.Shared.Math.Quaternion));
            engine.AddHostType("Matrix4", typeof(GrandTheftMultiplayer.Shared.Math.Matrix4));
            engine.AddHostType("WeaponHash", typeof(WeaponHash));
            engine.AddHostType("VehicleHash", typeof(VehicleHash));
            engine.AddHostType("PedHash", typeof(PedHash));
            engine.Script.host_typeof = new Action<dynamic>(arg => {
                API.consoleOutput("TYPE =" + engine);
            });
            engine.Script.require = new Func<dynamic, ScriptObject>(arg =>
            {
                return Import(arg);
            });
            engine.Execute("var modules = {};");
            return engine;
        }
        public void Init()
        {
            API.consoleOutput("Starting \"Javascript Connector\"!");
            string initFilePath = API.getSetting<string>("start");
            ExposeAPI(this.engine);
            Import(initFilePath);

            //benchmark();
        }
    }
}
