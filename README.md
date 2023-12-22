EasyStm Tool Is Made to Use StimulSoft In Easier Way And Clean Code

First Of All Add Below Config In AppSetting : 

              "Stimulsoft": {
                "License": "Your LicenseKey"
              }


Then :

Add wwwroot Folder In Api

wwwroot Contains Two Folder

fonts,reports 

Add Your StimulReport Files In reports

Add your fonts in fonts folder


Usage Example: 

            // TryEasyStmClass.cs

            using EasyStm.Models;
            using EasyStm.Reports.Pdf;
            
            namespace TryEasyStm;
            
            public class TryEasyStmClass : ITryEasyStmClass
            {
                private readonly IStmReports _stmReports;
            
            
                public TryEasyStmClass(IStmReports stmReports)
                {
                    _stmReports = stmReports;
                }
                public async Task<ReportResult> TryEasyStm_Single()
                {
            
                    // If You have One BusinessObject
            
                    var reportBusinessObject = new ReportObject<object>()
                    {
                        BusinessObjectName = "PersonDetails",
                        ObjectData = new { name = "name 1234" }
                    };

                    var FontsList = new List<ReportFont>() {
                        new ReportFont{ FontName = "FontName1"},
                        new ReportFont{ FontName = "FontName2"},
                    };
            
                    var info = new ReportInfo()
                    {
                        FontName = FontsList,
                        FontExtension = "ttf", // Defualt Is "ttf"
                        ReportFileName = "Stimul Report File Name.mrt",
                        PdfFileName = "pdf Result FileName",
                    };
            
                    var ReportResult = await _stmReports.PrintPDFAsync(reportBusinessObject, info);
            
                    return ReportResult;
                }
            
                public async Task<ReportResult> TryEasyStm_List()
                {
                    // If You have Multiple BusinessObject
            
                    var reportBusinessObjects = new List<ReportObject<object>>() {
                                new ReportObject<object>{ BusinessObjectName = "PersonDetails" , ObjectData = new{ name = "name 1234"} },
                                new ReportObject<object>{ BusinessObjectName = "JobDetails" , ObjectData = new{jobname="jobname 125788"} },
                                new ReportObject<object>{ BusinessObjectName = "CompanyDetails" , ObjectData = new{ companyname = "companyname 31598772"} }
                            };
            
                    var FontsList = new List<ReportFont>() {
                        new ReportFont{ FontName = "FontName1"},
                        new ReportFont{ FontName = "FontName2"},
                    };
            
                    var info = new ReportInfo()
                    {
                        FontName = FontsList,
                        FontExtension = "ttf", // Defualt Is "ttf"
                        ReportFileName = "Stimul Report File Name.mrt",
                        PdfFileName = "pdf Result FileName",
                    };
            
            
                    ReportResult ReportResult = await _stmReports.PrintPDFAsync(reportBusinessObjects, info);
                    return ReportResult;
                }
            }

            -------------------------------------------------------------------------------------------------------------------------------------------

            //Controller.cs

            private readonly ITryEasyStmClass _tryEasyStmClass;

            public tryStmController(ITryEasyStmClass tryEasyStmClass)
            {
                _tryEasyStmClass = tryEasyStmClass;
            }

            [HttpPost(nameof(GetReport))]
            public async Task<ReportResult> GetReport()
            {
                return await _tryEasyStmClass.TryEasyStm_List();
            }

            -------------------------------------------------------------------------------------------------------------------------------------------

            //Program.cs

            builder.Services.AddScoped<IStmReports, StmReports>();