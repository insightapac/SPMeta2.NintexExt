using Microsoft.SharePoint.Client;
using SPMeta2.NintexExt.Core.Definitions;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.NintexExt.Core.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.NintexExt.CSOM.O365.Handlers;

namespace SPMeta2.NintexExt.CSOM.O365.Test.Model
{
    public class WebModel
    {
        public static void Provision(ClientContext context, 
            CSOMProvisionService provisioningService)
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddNintexWorkflowO365(webWorkflow, workflowModel =>
                {
                    #region Events
                    workflowModel.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the workflow on the web");
                        });
                    workflowModel.OnProvisioned<NintexO365HandlerOnProvisionedEvent>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the workflow m for {0}", ((NintexWebWorkflowO365Definition)spMetaCtx.ObjectDefinition).Name);
                            var result = spMetaCtx.Object;
                            if (result.saveResponse != null)
                            {
                                Console.WriteLine("The result of save is {0}", result.saveResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.puiblishResponse != null)
                            {
                                Console.WriteLine("The result of publish is {0}", result.puiblishResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.assignedUseForProductionValue != null)
                            {
                                Console.WriteLine("The result of assigned use is {0}", result.assignedUseForProductionValue.Content.ReadAsStringAsync().Result);
                            }
                        });
                    #endregion
                });
                web.AddWeb(SubWeb, subweb => {
                    // this is not working this is the point to fix within the issue #11
                    //setupWeb(subweb);
                });
                setupWeb(web);
            });

            provisioningService.DeployWebModel(context, webModel);
        }

        private static void setupWeb(WebModelNode web)
        {
            web.AddList(TestList, list =>
            {
                list.AddContentTypeLink(BuiltInContentTypeId.Issue);
                // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                list.AddNintexFormO365(form, formmodel =>
                {
                    #region Events
                    formmodel.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                        });
                    formmodel.OnProvisioned<NintexO365HandlerOnProvisionedEvent>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            var result = spMetaCtx.Object;
                            if (result.saveResponse != null)
                            {
                                Console.WriteLine("The result of save is {0}", result.saveResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.puiblishResponse != null)
                            {
                                Console.WriteLine("The result of publish is {0}", result.puiblishResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.assignedUseForProductionValue != null)
                            {
                                Console.WriteLine("The result of assigned use is {0}", result.assignedUseForProductionValue.Content.ReadAsStringAsync().Result);
                            }
                        });
                    #endregion
                });
                // if you do not use the syntax default, you can use the line below
                //list.AddDefinitionNode(form);
                list.AddNintexWorkflowO365(listWorkflow, listWorkflow =>
                {
                    #region Events
                    listWorkflow.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the list workflow");
                        });
                    listWorkflow.OnProvisioned<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the list workflow");
                        });
                    //TODO:
                    //listWorkflow.OnProvisioned<NintexFormO365HandlerOnProvisionedEvent>
                    //    (spMetaCtx =>
                    //    {
                    //        Console.WriteLine("Provisoined the workflow m for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                    //        var result = spMetaCtx.Object;
                    //        if (result.saveResponse != null)
                    //        {
                    //            Console.WriteLine("The result of save is {0}", result.saveResponse.Content.ReadAsStringAsync().Result);
                    //        }
                    //        if (result.puiblishResponse != null)
                    //        {
                    //            Console.WriteLine("The result of publish is {0}", result.puiblishResponse.Content.ReadAsStringAsync().Result);
                    //        }
                    //        if (result.assignedUseForProductionValue != null)
                    //        {
                    //            Console.WriteLine("The result of assigned use is {0}", result.assignedUseForProductionValue.Content.ReadAsStringAsync().Result);
                    //        }
                    //    });
                    #endregion
                });
            });
            // same here, same list, testing that it works when the list is already there
            web.AddHostList(TestList, list =>
            {
                // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                list.AddNintexFormO365(form, formmodel =>
                {
                    #region Events
                    formmodel.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                        });
                    formmodel.OnProvisioned<NintexO365HandlerOnProvisionedEvent>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            var result = spMetaCtx.Object;
                            if (result.saveResponse != null)
                            {
                                Console.WriteLine("The result of save is {0}", result.saveResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.puiblishResponse != null)
                            {
                                Console.WriteLine("The result of publish is {0}", result.puiblishResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.assignedUseForProductionValue != null)
                            {
                                Console.WriteLine("The result of assigned use is {0}", result.assignedUseForProductionValue.Content.ReadAsStringAsync().Result);
                            }
                        });
                    #endregion
                });
                // if you do not use the syntax default, you can use the line below
                //list.AddDefinitionNode(form);
                list.AddNintexWorkflowO365(listWorkflow, listWorkflow =>
                {
                    #region Events
                    listWorkflow.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the list workflow");
                        });
                    listWorkflow.OnProvisioned<NintexO365HandlerOnProvisionedEvent>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the workflow m for {0}", ((NintexListWorkflowO365Definition)spMetaCtx.ObjectDefinition).Name);
                            var result = spMetaCtx.Object;
                            if (result.saveResponse != null)
                            {
                                Console.WriteLine("The result of save is {0}", result.saveResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.puiblishResponse != null)
                            {
                                Console.WriteLine("The result of publish is {0}", result.puiblishResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.assignedUseForProductionValue != null)
                            {
                                Console.WriteLine("The result of assigned use is {0}", result.assignedUseForProductionValue.Content.ReadAsStringAsync().Result);
                            }
                        });
                    #endregion
                });
            });
            web.AddList(TestList2, list =>
            {
                list.AddContentTypeLink(BuiltInContentTypeId.Issue);
                // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                list.AddNintexFormO365(form2_item, formmodel =>
                {
                    #region Events
                    formmodel.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                        });
                    formmodel.OnProvisioned<NintexO365HandlerOnProvisionedEvent>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            var result = spMetaCtx.Object;
                            if (result.saveResponse != null)
                            {
                                Console.WriteLine("The result of save is {0}", result.saveResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.puiblishResponse != null)
                            {
                                Console.WriteLine("The result of publish is {0}", result.puiblishResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.assignedUseForProductionValue != null)
                            {
                                Console.WriteLine("The result of assigned use is {0}", result.assignedUseForProductionValue.Content.ReadAsStringAsync().Result);
                            }
                        });
                    #endregion
                });
                list.AddNintexFormO365(form2_issue, formmodel =>
                {
                    #region Events
                    formmodel.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                        });
                    formmodel.OnProvisioned<NintexO365HandlerOnProvisionedEvent>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            var result = spMetaCtx.Object;
                            if (result.saveResponse != null)
                            {
                                Console.WriteLine("The result of save is {0}", result.saveResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.puiblishResponse != null)
                            {
                                Console.WriteLine("The result of publish is {0}", result.puiblishResponse.Content.ReadAsStringAsync().Result);
                            }
                            if (result.assignedUseForProductionValue != null)
                            {
                                Console.WriteLine("The result of assigned use is {0}", result.assignedUseForProductionValue.Content.ReadAsStringAsync().Result);
                            }
                        });
                    #endregion
                });
                list.AddNintexWorkflowO365(listWorkflow2, listWorkflow =>
                {
                    #region Events
                    listWorkflow.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the list workflow 2");
                        });
                    listWorkflow.OnProvisioned<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the list workflow 2");
                        });
                    #endregion
                });
            });
        }

        public static ListDefinition TestList = new ListDefinition()
        {
            TemplateType = BuiltInListTemplateTypeId.GenericList,
            CustomUrl = "Lists/Test",
            Title = "Test",
            ContentTypesEnabled = true,
            OnQuickLaunch = true
        };
        /// <summary>
        /// New list for testing lookups mapping
        /// </summary>
        public static ListDefinition TestList2 = new ListDefinition()
        {
            TemplateType = BuiltInListTemplateTypeId.GenericList,
            CustomUrl = "Lists/Test2",
            Title = "Test2",
            ContentTypesEnabled = true,
            OnQuickLaunch = true,
            EnableVersioning = true,
            EnableMinorVersions = false,
            EnableModeration = false
        };

        public static NintexFormO365Definition form = new NintexFormO365Definition()
        {
            ListContentTypeNameOrId = "Issue",
            FormData = System.IO.File.ReadAllBytes(@"Files\NintexForm.nfp"),
            Publish = true,
            AssignedUseForProduction = false
        };
        public static NintexFormO365Definition form2_item = new NintexFormO365Definition()
        {
            ListContentTypeNameOrId = "Item",
            FormData = System.IO.File.ReadAllBytes(@"Files\NintexForm2_item.nfp"),
            Publish = true,
            AssignedUseForProduction = false
        };
        public static NintexFormO365Definition form2_issue = new NintexFormO365Definition()
        {
            ListContentTypeNameOrId = "Issue",
            FormData = System.IO.File.ReadAllBytes(@"Files\NintexForm2_issue.nfp"),
            Publish = true,
            AssignedUseForProduction = false
        };


        public static NintexListWorkflowO365Definition listWorkflow = new NintexListWorkflowO365Definition()
        {
            WorkflowData = System.IO.File.ReadAllBytes(@"Files\ListWorkflow.nwp"),
            Publish = true,
            AssignedUseForProduction = false,
            Name = "List Workflow"
        };
        public static NintexListWorkflowO365Definition listWorkflow2 = new NintexListWorkflowO365Definition()
        {
            WorkflowData = System.IO.File.ReadAllBytes(@"Files\ListWorkflow2.nwp"),
            Publish = true,
            AssignedUseForProduction = false,
            Name = "List Workflow 2"
        };

        public static NintexWebWorkflowO365Definition webWorkflow = new NintexWebWorkflowO365Definition()
        {
            WorkflowData = System.IO.File.ReadAllBytes(@"Files\SiteWorkflow.nwp"),
            Publish = true,
            AssignedUseForProduction = false,
            Name = "Site Workflow",
        };

        public static WebDefinition SubWeb = new WebDefinition()
        {
            Title = "NintexTest",
            Url = "NintexTest",
            WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
        };
    }
}
