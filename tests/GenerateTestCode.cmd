@Echo off

Echo ================================================================
Echo Generating code: Read test model, AdventureWorks2008, C#
Echo ================================================================
"C:\users\frans\LLBLGen Pro v4.2\cligenerator.exe" "SourceProjects\AdventureWorks2008.llblgenproj" "ReadTestsAdventureWorks2008" "C#" ".NET 4.5" "General" "SD.LinqToSql" "AdventureWorks2008Model" 0 "SourceProjects\log_AdventureWorks2008.txt"
Echo ================================================================
Echo Generating code: Write test model, WriteTests, C#
Echo ================================================================
"C:\users\frans\LLBLGen Pro v4.2\cligenerator.exe" "SourceProjects\WriteTests.llblgenproj" "WriteTests" "C#" ".NET 4.5" "General" "SD.LinqToSql" "WriteTestsModel" 0 "SourceProjects\log_WriteTests.txt"
