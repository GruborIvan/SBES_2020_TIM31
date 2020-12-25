using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace SecurityManager
{
    public class RolesConfig
    {
        // putanja do RolesConfigFile.resx fajla
        static string path = @"";
        public static bool GetPermissions(string rolename, out string[] permissions)
        {
            //TO DO : izvuci sve permisije na osnovu prosledjene grupe
            permissions = new string[10];
            return false;

        }

        public static void AddPermissions(string rolename, string[] permissions)
        {
            //TO DO : dodavanje permisija u RolesConfigFile.resx
            var reader = new ResXResourceReader(path);
            var writer = new ResXResourceWriter(path);
        }

        public static void RemovePermissions(string rolename, string[] permissions)
        {
            //TO DO : brisanje permisija iz RolesConfigFile.resx
            var reader = new ResXResourceReader(path);            
            var writer = new ResXResourceWriter(path);          
        }

        public static void RemoveRole(string rolename)
        {
            //TO DO : brisanje rola iz RolesConfigFile.resx
            var reader = new ResXResourceReader(path);
            var writer = new ResXResourceWriter(path);
        }

        public static void AddRole(string rolename)
        {
            //TO DO : dodavanje rola u RolesConfigFile.resx
            var reader = new ResXResourceReader(path);
            var writer = new ResXResourceWriter(path);
        }
        
    }
}
