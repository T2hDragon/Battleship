#pragma checksum "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b75b7bcd53cded62ddd866e90747bde7fda1ba26"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WebApp.Pages.GamePlay.Pages_GamePlay_Index), @"mvc.1.0.razor-page", @"/Pages/GamePlay/Index.cshtml")]
namespace WebApp.Pages.GamePlay
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\_ViewImports.cshtml"
using WebApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
using System.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
using Domain.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
using GameBrain;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b75b7bcd53cded62ddd866e90747bde7fda1ba26", @"/Pages/GamePlay/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ab56b927680bad6a0ba24d2e6e24966a4e74b8b3", @"/Pages/_ViewImports.cshtml")]
    public class Pages_GamePlay_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary btn-block"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page-handler", "ExitToMainPage", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 6 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
  
    ViewData["Title"] = $"Game {Model.BattleShip.Game.Name}";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 84 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
 if (Model.BattleShip.GameHasEnded())
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p class=\"h2 text-center\">\r\n        <u> Player ");
#nullable restore
#line 87 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
              Write(Model.BattleShip.GetCurrentPlayer().Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Won! </u>\r\n    </p>\r\n");
            WriteLiteral("    <div class=\"row\">\r\n");
#nullable restore
#line 91 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
         foreach (var player in Model.BattleShip.GetPlayers())
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"col text-center\">\r\n                <table class=\"table-bordered game-table\">\r\n");
#nullable restore
#line 95 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                     for (var y = 0; y < Model.Height; y++)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr class=\"m-0\">\r\n");
#nullable restore
#line 98 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                             for (var x = 0; x <= Model.Width; x++)
                            {
                                if (x == 0 || y == 0)
                                {
                                    if (x == y)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 104 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                       Write(Html.Raw("&nbsp;"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 105 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                    else if (x == 0)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 108 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                        Write(y - 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 109 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                    else if (y == 0)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 112 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                       Write(Model.Alphabet[x - 1]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 113 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                }
                                else
                                {
                                    var cellString = GetCellContent(player, (x - 1, y - 1), false);

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <td");
            BeginWriteAttribute("class", " class=\"", 4504, "\"", 4545, 2);
#nullable restore
#line 118 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
WriteAttributeValue("", 4512, GetTDClass(cellString), 4512, 23, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 4535, "game-cell", 4536, 10, true);
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 118 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                                                             Write(Html.Raw(cellString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 119 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                }
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tr>\r\n");
#nullable restore
#line 122 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </table>\r\n            </div>\r\n");
#nullable restore
#line 125 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
#nullable restore
#line 127 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
    await Model.BattleShip.DeleteFromDatabase(Model.BattleShip.GameId);
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p class=\"h2 text-center\">\r\n        <u> Player ");
#nullable restore
#line 132 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
              Write(Model.BattleShip.GetCurrentPlayer().Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(" turn! </u>\r\n    </p>\r\n    <div class=\"row\">\r\n        <div class=\"col-4\">\r\n            <div class=\"btn btn-primary btn-block\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5066, "\"", 5157, 7);
            WriteAttributeValue("", 5076, "window.location.href", 5076, 20, true);
            WriteAttributeValue(" ", 5096, "=", 5097, 2, true);
            WriteAttributeValue(" ", 5098, "\'?gameId=", 5099, 10, true);
#nullable restore
#line 136 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
WriteAttributeValue("", 5108, Model.BattleShip.GameId, 5108, 24, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5132, "&hidden=", 5132, 8, true);
#nullable restore
#line 136 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
WriteAttributeValue("", 5140, !Model.Hidden, 5140, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5156, "\'", 5156, 1, true);
            EndWriteAttribute();
            WriteLiteral("\r\n                 style=\"cursor: pointer\">\r\n                ");
#nullable restore
#line 138 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
            Write(Model.Hidden ? "Show" : "Hide");

#line default
#line hidden
#nullable disable
            WriteLiteral(" My Board\r\n            </div>\r\n\r\n            <div class=\"btn btn-primary btn-block\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5335, "\"", 5412, 5);
            WriteAttributeValue("", 5345, "window.location.href", 5345, 20, true);
            WriteAttributeValue(" ", 5365, "=", 5366, 2, true);
            WriteAttributeValue(" ", 5367, "\'?gameId=", 5368, 10, true);
#nullable restore
#line 141 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
WriteAttributeValue("", 5377, Model.BattleShip.GameId, 5377, 24, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5401, "&undo=true\'", 5401, 11, true);
            EndWriteAttribute();
            WriteLiteral("\r\n                 style=\"cursor: pointer\">\r\n                Undo\r\n            </div>\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b75b7bcd53cded62ddd866e90747bde7fda1ba2613149", async() => {
                WriteLiteral("Save&Exit");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.PageHandler = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </div>\r\n");
#nullable restore
#line 147 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
         if (!Model.Hidden)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"col text-center\">\r\n                <table class=\"table-bordered\">\r\n");
#nullable restore
#line 151 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                     for (var y = 0; y <= Model.Height; y++)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr class=\"m-0\">\r\n");
#nullable restore
#line 154 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                             for (var x = 0; x <= Model.Width; x++)
                            {
                                if (x == 0 || y == 0)
                                {
                                    if (x == y)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 160 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                       Write(Html.Raw("&nbsp;"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 161 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                    else if (x == 0)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 164 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                        Write(y - 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 165 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                    else if (y == 0)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 168 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                       Write(Model.Alphabet[x - 1]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 169 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                }
                                else
                                {
                                    var cellString = GetCellContent(Model.BattleShip.GetCurrentPlayer(), (x - 1, y - 1), false);

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <td");
            BeginWriteAttribute("class", " class=\"", 6932, "\"", 6973, 2);
#nullable restore
#line 174 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
WriteAttributeValue("", 6940, GetTDClass(cellString), 6940, 23, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 6963, "game-cell", 6964, 10, true);
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 174 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                                                             Write(Html.Raw(cellString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 175 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                }
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tr>\r\n");
#nullable restore
#line 178 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </table>\r\n            </div>\r\n");
#nullable restore
#line 181 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
            WriteLiteral("    <div class=\"row\">\r\n\r\n");
#nullable restore
#line 186 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
         foreach (var player in Model.BattleShip.GetPlayerOpponents(Model.BattleShip.GetPlayers().IndexOf(Model.BattleShip.GetCurrentPlayer())))
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <br>\r\n            <div class=\"col text-center\">\r\n                <p class=\"align-middle\">Player ");
#nullable restore
#line 190 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                          Write(player.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(" board</p>\r\n                <table class=\"table-bordered game-table\">\r\n");
#nullable restore
#line 192 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                     for (var y = 0; y <= Model.Height; y++)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr class=\"m-0\">\r\n");
#nullable restore
#line 195 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                             for (var x = 0; x <= Model.Width; x++)
                            {
                                if (x == 0 || y == 0)
                                {
                                    if (x == y)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 201 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                       Write(Html.Raw("&nbsp;"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 202 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                    else if (x == 0)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 205 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                        Write(y - 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 206 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                    else if (y == 0)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>");
#nullable restore
#line 209 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                       Write(Model.Alphabet[x - 1]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 210 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                    }
                                }
                                else
                                {
                                    var cellString = GetCellContent(player, (x - 1, y - 1), true);

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <td");
            BeginWriteAttribute("class", " class=\"", 8724, "\"", 8765, 2);
#nullable restore
#line 215 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
WriteAttributeValue("", 8732, GetTDClass(cellString), 8732, 23, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 8755, "game-cell", 8756, 10, true);
            EndWriteAttribute();
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 216 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                         if (cellString == "&nbsp;")
                                        {
                                            

#line default
#line hidden
#nullable disable
            WriteLiteral("onclick = \"window.location.href = \'?gameId=");
#nullable restore
#line 218 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                                                                        Write(Model.BattleShip.GameId);

#line default
#line hidden
#nullable disable
            WriteLiteral("&x=");
#nullable restore
#line 218 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                                                                                                    Write(x - 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("&y=");
#nullable restore
#line 218 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                                                                                                               Write(y - 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("&boardId=");
#nullable restore
#line 218 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                                                                                                                               Write(player.PlayerBoard.GameBoardId);

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\"\r\n                                                style = \"cursor: pointer\"");
#nullable restore
#line 219 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                                                                
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(">\r\n                                        ");
#nullable restore
#line 221 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                   Write(Html.Raw(cellString));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n");
#nullable restore
#line 223 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                                }
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tr>\r\n");
#nullable restore
#line 226 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </table>\r\n            </div>\r\n");
#nullable restore
#line 229 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
#nullable restore
#line 231 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 11 "C:\Users\Karmo\Desktop\C#\C# gameDev\icd0008-2020f\Battleship\WebApp\Pages\GamePlay\Index.cshtml"
 
    string GetCellContent(Player player, (int x, int y) location, bool boatsHidden)
    {
        var hasBoatInLocation = player.HasBoatInLocation(location);
        var hasPlacementBoatInLocation = player.BoatBeingPlaced != null &&
                                         player.BoatBeingPlaced.GetCellLocations()
                                             .Contains(location);
        var eCellState = player.PlayerBoard.GetCellState(location);
        var cellString = eCellState switch
        {
            ECellState.Empty => "&nbsp;",
            ECellState.Hit => "X",
            ECellState.Miss => "O",
            _ => throw new InvalidExpressionException($"Asked unknown cellState: {eCellState}")
            };
        if (player.HasBoatSunkInLocation(location))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            cellString = "#";
        }
        else if (player.IsLocationLocked(location, Model.BattleShip.GetBoatsCanTouch()))
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            cellString = "*";
        }
    //Modify CellState presentation based on ship locations and game rules
        else if (!boatsHidden && (hasBoatInLocation || hasPlacementBoatInLocation) && eCellState == ECellState.Empty)
        {
            var possibleBoat = hasBoatInLocation
                ? player.Boats.First(boat => boat.GetCellLocations().Contains(location))
                : player.GetBoatBeingPlaced();
            var index = possibleBoat.GetCellLocations().IndexOf(location);
            cellString = index == 0 ? "<" : index != possibleBoat.GetCellLocations().Count() - 1 ? "S" : ">";
        }
        return cellString;
    }

    string GetTDClass(string cellString)
    {
        var result = " font-weight-bold .col m-0 bg-primary";
        switch (cellString)
        {
            case "&nbsp;":
                break;
            case "#":
                result = $"{result} text-warning bg-danger";
                break;
            case "X":
                result = $"{result} text-danger";
                break;
            case "*":
                result = $"{result} text-secondary";
                break;
            case "S":
                result = $"{result} text-dark";
                break;
            case "<":
                result = $"{result} text-dark";
                break;
            case ">":
                result = $"{result} text-dark";
                break;
            case "O":
                result = $"{result} text-info";
                break;
            default:
                throw new InvalidOperationException($"Unknown cell string:\"{cellString}\"");
        }

        return result;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WebApp.Pages.GamePlay.Index> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<WebApp.Pages.GamePlay.Index> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<WebApp.Pages.GamePlay.Index>)PageContext?.ViewData;
        public WebApp.Pages.GamePlay.Index Model => ViewData.Model;
    }
}
#pragma warning restore 1591