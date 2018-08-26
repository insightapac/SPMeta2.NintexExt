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
                    // this refers to SPMeta2.NintexExt.Core.Syntax.Default;
                    list.AddNintexFormO365(form, formmodel =>
                    {
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
            ListContentTypeNameOrId = "Item",
            FormData = System.IO.File.ReadAllBytes(@"Files\NintexForm.nfp")
        };

    }
}
