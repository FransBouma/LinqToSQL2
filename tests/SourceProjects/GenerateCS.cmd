@Echo off
cd %1

Echo ####################################
Echo Linq builds for vs.net 2010
Echo ####################################
Echo ================================================================
Echo Generating code: Adapter 2005, C#
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\Northwind26.llblgenproj" "NW26.Adapter" "C#" ".NET 3.5" "Adapter" "SD.Presets.Adapter.General" "%~2\AdapterDAL" 0 "%~2\LLBLGenPro Projects\log_NW26_Adapter.txt"
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\Northwind26.llblgenproj" "NW26Async.Adapter" "C#" ".NET 4.5" "Adapter" "SD.Presets.Adapter.General" "%~2\AdapterDALAsync" 0 "%~2\LLBLGenPro Projects\log_NW26_Adapter_Async.txt"
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\AdventureWorksUnitTests.llblgenproj" "AdventureWorks.Dal.Adapter" "C#" ".NET 3.5" "Adapter" "SD.Presets.Adapter.General" "%~2\AdapterAdventureWorksDAL" 0 "%~2\LLBLGenPro Projects\log_AdventureWorks_Adapter.txt"
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\InheritanceTwo.llblgenproj" "InheritanceTwo.Adapter" "C#" ".NET 3.5" "Adapter" "SD.Presets.Adapter.General" "%~2\AdapterIH2DAL" 0 "%~2\LLBLGenPro Projects\log_InheritanceTwo_Adapter.txt"
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\Scott10g.llblgenproj" "Scott10g" "C#" ".NET 3.5" "Adapter" "SD.Presets.Adapter.General" "%~2\AdapterScott10g" 0 "%~2\LLBLGenPro Projects\log_Scott10g_Adapter.txt"

Echo ================================================================
Echo Generating code: SelfServicing 2-class 2010, C#
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\Northwind26.llblgenproj" "NW26.SelfServicing" "C#" ".NET 3.5" "SelfServicing" "SD.Presets.SelfServicing.TwoClasses" "%~2\SelfServicingNWDAL" 0 "%~2\LLBLGenPro Projects\log_NW26_SelfServicing.txt"
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\Northwind26.llblgenproj" "NW26Async.SelfServicing" "C#" ".NET 4.5" "SelfServicing" "SD.Presets.SelfServicing.TwoClasses" "%~2\SelfServicingNWDALAsync" 0 "%~2\LLBLGenPro Projects\log_NW26_SelfServicing_Async.txt"
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\InheritanceTwo.llblgenproj" "InheritanceTwo.SelfServicing" "C#" ".NET 3.5" "SelfServicing" "SD.Presets.SelfServicing.TwoClasses" "%~2\SelfServicingIH2DAL" 0 "%~2\LLBLGenPro Projects\log_InheritanceTwo_SelfServicing.txt"
Echo ================================================================
cligenerator "%~2\LLBLGenPro Projects\Scott10g.llblgenproj" "Scott10g.SelfServicing" "C#" ".NET 3.5" "SelfServicing" "SD.Presets.SelfServicing.General" "%~2\SelfServicingScott10g" 0 "%~2\LLBLGenPro Projects\log_Scott10g_SelfServicing.txt"
