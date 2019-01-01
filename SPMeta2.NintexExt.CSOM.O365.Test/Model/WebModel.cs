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
                web.AddList(TestList, list =>
                {
                    list.AddContentTypeLink(BuiltInContentTypeId.Issue);
                    // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                    list.AddNintexFormO365(form, formmodel =>
                    {
                        formmodel.OnProvisioning<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("About to provision the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            });
                        formmodel.OnProvisioned<NintexFormO365HandlerOnProvisionedEvent>
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
                    });
                // if you do not use the syntax default, you can use the line below
                //list.AddDefinitionNode(form);
                list.AddNintexWorkflowO365(listWorkflow, listWorkflow => {
                    listWorkflow.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the workflow");
                        });
                    listWorkflow.OnProvisioned<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisoined the workflow");
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
                });
                });
                // same here, same list, testing that it works when the list is already there
                web.AddHostList(TestList, list =>
                {
                    // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                    list.AddNintexFormO365(form, formmodel =>
                    {
                        formmodel.OnProvisioning<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("About to provision the form for {0}", ((NintexFormO365Definition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            });
                        formmodel.OnProvisioned<NintexFormO365HandlerOnProvisionedEvent>
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
                    });
                    // if you do not use the syntax default, you can use the line below
                    //list.AddDefinitionNode(form);
                    list.AddNintexWorkflowO365(listWorkflow, listWorkflow => {
                        listWorkflow.OnProvisioning<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("About to provision the workflow");
                            });
                        listWorkflow.OnProvisioned<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("Provisoined the workflow");
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
                    });
                });
            });

            provisioningService.DeployWebModel(context, webModel);
        }

        public static ListDefinition TestList = new ListDefinition()
        {
            TemplateType = BuiltInListTemplateTypeId.GenericList,
            CustomUrl = "Lists/Test",
            Title = "Test",
            ContentTypesEnabled = true
        };

        public static NintexFormO365Definition form = new NintexFormO365Definition()
        {
            ListContentTypeNameOrId = "Issue",
            FormData = System.IO.File.ReadAllBytes(@"Files\NintexForm.nfp"),
            Publish = true,
            AssignedUseForProduction = false
        };


        public static NintexListWorkflowO365Definition listWorkflow = new NintexListWorkflowO365Definition()
        {
            WorkflowData = System.IO.File.ReadAllBytes(@"Files\ListWorkflow.nwp"),
            Publish = true,
            AssignedUseForProduction = false,
            WorkflowId = "317caa78-8377-479d-a8ea-a04ceefb4bde"
        };

        public static NintexWebWorkflowO365Definition siteWorkflow = new NintexWebWorkflowO365Definition()
        {
            WorkflowData = System.IO.File.ReadAllBytes(@"Files\SiteWorkflow.nwp"),
            Publish = true,
            AssignedUseForProduction = false,
            WorkflowId = "b3c00e47-2473-4ac4-9c12-5a2fc22bb80e"
        };
    }
}
