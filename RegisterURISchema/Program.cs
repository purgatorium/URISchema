// reference: https://msdn.microsoft.com/en-us/library/aa767914(v=vs.85).aspx

using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Security.Principal;
using System.Reflection;



namespace RegisterURISchema
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (false == IsElevatedUser)
            {
                if (false == ExecuteElevated())
                {
                    MessageBox.Show("관리자 권한을 허가해야 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 프로그램 종료
                return;
            }

            if (false == WriteRegistry())
            {
                MessageBox.Show("레지스트리 권한문제?", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        static bool IsElevatedUser
        {
            // 현재 사용자가 관리자 권한이 있는지 확인한다.
            get { return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator); }
        }


        static bool ExecuteElevated()
        {
            // 관리자 권한으로 새로 실행한다.
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Verb = "runas";
            psi.FileName = Assembly.GetEntryAssembly().Location;
            try
            {
                Process.Start(psi);
                return true;
            }
            catch { }

            return false;
        }


        static bool WriteRegistry()
        {
            try
            {
                // 로컬호스트 관리자그룹을 사용해서 레지스트리 쓰기 권한을 할당한다.
                // 도메인에 속하는 컴퓨터인 경우 아래 코드가 잘 될지 안될지 모르겠다.
                RegistryAccessRule allow_write_rule = new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null), RegistryRights.WriteKey, AccessControlType.Allow);
                RegistrySecurity securitySettings = new RegistrySecurity();
                securitySettings.AddAccessRule(allow_write_rule);

                // MSDN의 샘플은 HKEY_CLASSROOT 에 저장하게 되어있는데
                // HKEY_CURRENT_USER 에 저장하는걸 권장한다고 하더라.
                // 난 그냥 HKEY_LOCAL_MACHINE 에 저장한다.
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey);
                key.SetAccessControl(securitySettings);

                // 위에서 권한설정을 했으니 아래에 security 설정하는 부분은 사실상 필요 없다.

                // 스키마 생성
                RegistryKey reg_base = key.CreateSubKey("mytro.RecPlayer", RegistryKeyPermissionCheck.Default, RegistryOptions.None, securitySettings);
                if (null == reg_base)
                    return false;
                // 기본값 설정
                reg_base.SetValue("", "mytro record player URI", RegistryValueKind.String);
                // 이게 꼭 필요한지는 모르겠음
                reg_base.SetValue("Content Type", "application/x-mytrorec", RegistryValueKind.String);
                // 이게 있어야 브라우저에서 클릭했을때 동작한다.
                reg_base.SetValue("URL Protocol", "", RegistryValueKind.String);


                // icon 설정
                RegistryKey icon = reg_base.CreateSubKey("DefaultIcon", RegistryKeyPermissionCheck.Default, RegistryOptions.None, securitySettings);
                if (null == icon)
                    return false;
                icon.SetValue("", @"C:\MYTRO\RecordPlayer\MytroPlayer.exe,1", RegistryValueKind.String);


                // 핸들러 설정 (shell open 을 만들어준다.)
                RegistryKey shell = reg_base.CreateSubKey("shell", RegistryKeyPermissionCheck.Default, RegistryOptions.None, securitySettings);
                if (null == shell)
                    return false;
                shell.SetValue("", "open", RegistryValueKind.String);


                RegistryKey open = shell.CreateSubKey("open", RegistryKeyPermissionCheck.Default, RegistryOptions.None, securitySettings);
                if (null == open)
                    return false;


                RegistryKey command = open.CreateSubKey("command", RegistryKeyPermissionCheck.Default, RegistryOptions.None, securitySettings);
                if (null == command)
                    return false;

                // 실행파일도 Environment.SpecialFolder.ApplicationData 에 넣고 사용하는걸 권장하는데 왜 그런지는 잘 모르겠음...
                // 저 규칙에 따른다면 경로는 이렇게 된다. C:/Users/홍준기/AppData/Roaming/RecordPlayer/MytroPlayer.exe
                command.SetValue("", @"""C:\MYTRO\RecordPlayer\MytroPlayer.exe"" ""%1""", RegistryValueKind.String);

                return true;
            }
            catch { }
            return false;
        }
    }
}
