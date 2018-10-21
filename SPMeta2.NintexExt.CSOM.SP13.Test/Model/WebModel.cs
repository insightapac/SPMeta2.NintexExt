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

namespace SPMeta2.NintexExt.CSOM.SP13.Test.Model
{
    public class WebModel
    {
        public static void Provision(ClientContext context, CSOMProvisionService provisioningService)
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddNintexWorkflow(webWorkflow, workflowModel =>

                {
                    workflowModel.OnProvisioning<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("About to provision the workflow on the web");
                        });
                    workflowModel.OnProvisioned<Object>
                        (spMetaCtx =>
                        {
                            Console.WriteLine("Provisioned the web workflow");
                            Console.WriteLine("The result is {0}", spMetaCtx.Object);
                        });
                });

                web.AddList(TestList, list =>
                {
                    // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                    list.AddNintexForm(form, formmodel=> {
                        formmodel.OnProvisioning<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("About to provision the form for {0}", ((NintexFormDefinition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            });
                        formmodel.OnProvisioned<string>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("Provisoined the form for {0}",((NintexFormDefinition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                                Console.WriteLine("The result is {0}", spMetaCtx.Object);
                            });
                    });
                    // if you do not use the syntax default, you can use the line below
                    //list.AddDefinitionNode(form);


                    list.AddNintexWorkflow(listWorkflow, workflowModel =>

                    {
                        workflowModel.OnProvisioning<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("About to provision the workflow on the list");
                            });
                        workflowModel.OnProvisioned<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("Provisioned the workflow");
                                Console.WriteLine("The result is {0}", spMetaCtx.Object);
                            });
                    });

                });
                // same here, same list, testing that it works when the list is already there
                web.AddHostList(TestList, list =>
                {
                    // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                    list.AddNintexForm(form, formmodel => {
                        formmodel.OnProvisioning<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("About to provision the form for {0}", ((NintexFormDefinition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                            });
                        formmodel.OnProvisioned<string>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("Provisoined the form for {0}", ((NintexFormDefinition)spMetaCtx.ObjectDefinition).ListContentTypeNameOrId);
                                Console.WriteLine("The result is {0}", spMetaCtx.Object);
                            });
                    });
                    // if you do not use the syntax default, you can use the line below
                    //list.AddDefinitionNode(form);

                    list.AddNintexWorkflow(listWorkflow, workflowModel =>
                    {
                        workflowModel.OnProvisioning<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("About to provision the workflow on the list");
                            });
                        workflowModel.OnProvisioned<Object>
                            (spMetaCtx =>
                            {
                                Console.WriteLine("Provisioned the workflow");
                                Console.WriteLine("The result is {0}", spMetaCtx.Object);
                            });
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

        public static NintexFormDefinition form = new NintexFormDefinition()
        {
            ListContentTypeNameOrId = "Item",
            //ListContentTypeNameOrId = "0x0100BA82E9E260029149B08C0DEB97F100A5",
            FormXml = System.IO.File.ReadAllText(@"Files\ItemForm.xml", Encoding.Unicode)
        };

        public static NintexListWorkflowDefinition listWorkflow = new NintexListWorkflowDefinition()
        {
            Name = "Newly Deployed Workflow",
            WorkflowXml = System.IO.File.ReadAllText(@"Files\SampleListWorkflow.nwf", Encoding.UTF8)
        };

        public static NintexWebWorkflowDefinition webWorkflow = new NintexWebWorkflowDefinition()
        {
            Name = "Newly Deployed Web Workflow",
            WorkflowXml = System.IO.File.ReadAllText(@"Files\SampleSitetWorkflow", Encoding.UTF8)
        };

    }
}
