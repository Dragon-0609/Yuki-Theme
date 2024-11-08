﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 915 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeCompletion;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using PascalABCCompiler;

namespace YukiTheme.Style.CodeCompletion;

internal class TipPainterTools
{
	private const int SpacerSize = 4;

	// btw. I know it's ugly.
	public static Rectangle DrawingRectangle1;
	public static Rectangle DrawingRectangle2;

	private TipPainterTools()
	{
	}

	public static Size GetDrawingSizeHelpTipFromCombinedDescription(Control control,
		Graphics graphics,
		Font font,
		string countMessage,
		string description,
		int param_num, bool addit_info)
	{
		string basicDescription = null;
		string documentation = null;
		var bold_beg = -1;
		var bold_len = 0;
		if (IsVisibleText(description))
		{
			var splitDescription = description.Split(new[] { '\n' }, 2);

			if (splitDescription.Length > 0)
			{
				basicDescription = splitDescription[0];
				var extensionIndex = basicDescription.IndexOf("(расширение") + 1;
				if (param_num == 1)
				{
					bold_beg = basicDescription.IndexOf('(') + 1;
					if (bold_beg != 0 && bold_beg == extensionIndex)
						bold_beg = basicDescription.IndexOf('(', basicDescription.IndexOf(')') + 1) + 1;
					if (bold_beg != 0)
					{
						var end = basicDescription.IndexOf(CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter);
						var end_sk = basicDescription.IndexOf(')', bold_beg);
						if (end > end_sk || end == -1) end = end_sk;
						//if (end == -1) end = basicDescription.IndexOf(')');
						if (end != -1) bold_len = end - bold_beg;
					}
				}
				else
				{
					var i = 1;
					bold_beg = 0;
					while (i < param_num)
					{
						bold_beg = basicDescription.IndexOf(CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter, bold_beg) + 1;
						if (bold_beg == 0) break;
						i++;
					}

					if (bold_beg != 0)
					{
						var end = basicDescription.IndexOf(CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter, bold_beg);
						var end_sk = basicDescription.IndexOf(')', bold_beg);
						if (end > end_sk || end == -1) end = end_sk;
						if (end != -1) bold_len = end - bold_beg;
					}
				}

				if (splitDescription.Length > 1) documentation = splitDescription[1].Trim();
			}
		}

		return GetDrawingSizeDrawHelpTip(control, graphics, font, countMessage, basicDescription, documentation, bold_beg, bold_len, param_num, addit_info);
	}

	public static Size DrawHelpTipFromCombinedDescription(Control control,
		Graphics graphics,
		Font font,
		string countMessage,
		string description, int param_num, bool addit_info)
	{
		string basicDescription = null;
		string documentation = null;
		var bold_beg = -1;
		var bold_len = 0;
		if (IsVisibleText(description))
		{
			var splitDescription = description.Split
				(new[] { '\n' }, 2);

			if (splitDescription.Length > 0)
			{
				basicDescription = splitDescription[0];
				var extensionIndex = basicDescription.IndexOf("(расширение") + 1;
				if (param_num == 1)
				{
					bold_beg = basicDescription.IndexOf('(') + 1;
					if (bold_beg != 0 && bold_beg == extensionIndex)
						bold_beg = basicDescription.IndexOf('(', basicDescription.IndexOf(')') + 1) + 1;
					if (bold_beg != 0)
					{
						var end = basicDescription.IndexOf(CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter);
						var end_sk = basicDescription.IndexOf(')', bold_beg);
						if (end > end_sk || end == -1) end = end_sk;
						//if (end == -1) end = basicDescription.IndexOf(')');
						if (end != -1) bold_len = end - bold_beg;
					}
				}
				else
				{
					var i = 1;
					bold_beg = 0;
					while (i < param_num)
					{
						bold_beg = basicDescription.IndexOf(CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter, bold_beg) + 1;
						if (bold_beg == 0) break;
						i++;
					}

					if (bold_beg != 0)
					{
						var end = basicDescription.IndexOf(CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter, bold_beg);
						var end_sk = basicDescription.IndexOf(')', bold_beg);
						if (end > end_sk || end == -1) end = end_sk;
						if (end != -1) bold_len = end - bold_beg;
					}
				}

				if (splitDescription.Length > 1) documentation = splitDescription[1].Trim();
			}
		}

		return DrawHelpTip(control, graphics, font, countMessage,
			basicDescription, documentation, bold_beg, bold_len, param_num, addit_info);
	}

	public static Size GetDrawingSizeDrawHelpTip(Control control,
		Graphics graphics, Font font,
		string countMessage,
		string basicDescription,
		string documentation,
		int beg_bold, int bold_len, int param_num, bool addit_info)
	{
		if (IsVisibleText(countMessage) ||
		    IsVisibleText(basicDescription) ||
		    IsVisibleText(documentation))
		{
			// Create all the TipSection objects.
			var countMessageTip = new CountTipText(graphics, font, countMessage);

			var countSpacer = new TipSpacer(graphics, new SizeF(IsVisibleText(countMessage) ? 4 : 0, 0));

			var descriptionTip = new TipText(graphics, font, basicDescription);
			descriptionTip.insight_wnd = control is PABCNETInsightWindow;
			descriptionTip.beg_bold = beg_bold;
			descriptionTip.len_bold = bold_len;
			var descriptionTip1 = new TipText(graphics, font, basicDescription.Substring(0, beg_bold));
			var descriptionTip2 = new TipText(graphics, new Font(font, FontStyle.Bold), basicDescription.Substring(beg_bold, bold_len));
			var descriptionTip3 = new TipText(graphics, font, basicDescription.Substring(beg_bold + bold_len));
			descriptionTip.desc1 = descriptionTip1;
			descriptionTip.desc2 = descriptionTip2;
			descriptionTip.desc3 = descriptionTip3;
			var docSpacer = new TipSpacer(graphics, new SizeF(0, IsVisibleText(documentation) ? 4 : 0));

			var sections = new List<TipSection>();
			if (documentation != null)
				try
				{
					var params_ind = documentation.IndexOf("<params>");
					TipText paramsTextTipHeader = null;
					TipText paramsTextTip = null;
					if (params_ind != -1 && addit_info)
					{
						paramsTextTipHeader = new TipText(graphics, new Font(font, FontStyle.Bold), StringResources.Get("CODE_COMPLETION_PARAMETERS"));
						paramsTextTipHeader.is_head = true;
						var end_param = documentation.IndexOf("<returns>", params_ind);
						string params_text;
						if (end_param == -1)
							params_text = documentation.Substring(params_ind + 8).Trim(' ', '\n', '\r', '\t');
						//paramsTextTip = new TipText(graphics,font,params_text);
						else
							params_text = documentation.Substring(params_ind + 8, end_param - params_ind - 8).Trim(' ', '\n', '\r', '\t');
						//paramsTextTip = new TipText(graphics,font,documentation.Substring(params_ind+8,end_param-params_ind-8).Trim(' ','\n','\r','\t'));
						var prms_list = new List<string>();
						var ind = params_text.IndexOf("<param>", 0);
						while (ind != -1)
						{
							var end_ind = params_text.IndexOf("</param>", ind + 1);
							prms_list.Add(params_text.Substring(ind + 7, end_ind - ind - 7).Trim(' ', '\n', '\t', '\r'));
							ind = params_text.IndexOf("<param>", end_ind);
							if (ind != -1)
							{
								if (ind - end_ind - 7 > 0)
									prms_list.Add(params_text.Substring(end_ind + 8, ind - end_ind - 8).Trim(' ', '\n', '\t', '\r'));
								else
									prms_list.Add("");
							}
							else
							{
								prms_list.Add(params_text.Substring(end_ind + 8).Trim(' ', '\n', '\t', '\r'));
							}
						}

						var prm_sections = new List<TipSection>();
						var i = 0;
						while (i < prms_list.Count)
						{
							var prm_name_sect = new TipText(graphics, font, prms_list[i]);
							prm_name_sect.param_name = true;
							var desc_param_sect = new TipText(graphics, font, prms_list[i + 1]);
							desc_param_sect.need_tab = true;
							desc_param_sect.param_desc = true;
							//TipSplitter spltr = new TipSplitter(graphics,true,prm_name_sect,desc_param_sect);
							if (!string.IsNullOrEmpty(prms_list[i + 1]))
							{
								prm_sections.Add(prm_name_sect);
								prm_sections.Add(desc_param_sect);
							}

							i += 2;
						}

						if (prm_sections.Count > 0)
						{
							sections.Add(new TipText(graphics, new Font(font.FontFamily, 3), " "));
							sections.Add(paramsTextTipHeader);
							sections.AddRange(prm_sections);
						}
					}

					var return_ind = documentation.IndexOf("<returns>");
					/*TipText returnTextTipHeader = null;
					TipText returnTextTip = null;
					if (return_ind != -1 && addit_info)
					{
					    returnTextTipHeader = new TipText(graphics,new Font(font, FontStyle.Bold),PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN"));
					    returnTextTipHeader.is_head = true;
					    int end_return = documentation.IndexOf("<params>",return_ind);
					    string return_text;
					    if (end_return == -1)
					        return_text = documentation.Substring(return_ind+9).Trim(' ','\n','\r','\t');
					    else
					        return_text = documentation.Substring(return_ind+9,end_return-return_ind-9).Trim(' ','\n','\r','\t');
				
					    if (!string.IsNullOrEmpty(return_text))
					    {
					        returnTextTip = new TipText(graphics,font,return_text);
					        sections.Add(new TipText(graphics,new Font(font.FontFamily,3)," "));
					        sections.Add(returnTextTipHeader);
					        sections.Add(returnTextTip);
					        returnTextTip.need_tab = true;
					    }
					}*/
					if (params_ind != -1 && return_ind != -1)
						documentation = documentation.Substring(0, Math.Min(params_ind, return_ind)).Trim(' ', '\n', '\t', '\r');
					else if (params_ind != -1)
						documentation = documentation.Substring(0, params_ind).Trim(' ', '\n', '\t', '\r');
					else if (return_ind != -1)
						documentation = documentation.Substring(0, return_ind).Trim(' ', '\n', '\t', '\r');
				}
				catch
				{
				}

			var docTip = new TipText(graphics, font, documentation);
			//docTip.is_doc = true;
			docTip.need_tab = true;
			sections.Insert(0, docTip);
			if (!string.IsNullOrEmpty(documentation))
			{
				var descr_head = new TipText(graphics, new Font(font, FontStyle.Bold), StringResources.Get("CODE_COMPLETION_DESCRIPTION"));
				descr_head.is_head = true;
				sections.Insert(0, descr_head);
				sections.Insert(0, new TipText(graphics, new Font(font.FontFamily, 1), " "));
			}

			//docTip.is_doc = true;
			docTip.need_tab = true;
			var descSplitter = new TipSplitter(graphics, true,
				descriptionTip,
				docSpacer
			);
			// Now put them together.
			/*TipSplitter descSplitter = new TipSplitter(graphics, true,
			                                           //descriptionTip,
			                                           descriptionTip1,
			                                           descriptionTip2,
			                                           descriptionTip3,
			                                           docSpacer
			                                           );*/
			descSplitter.is_desc = true;
			/*TipSplitter descSplitter1 = new TipSplitter(graphics, false,
			                                           //descriptionTip,
			                                           descriptionTip1
			                                           );
			
			TipSplitter descSplitter2 = new TipSplitter(graphics, false,
			                                           //descriptionTip,
			                                           descriptionTip2
			                                           );
			
			TipSplitter descSplitter3 = new TipSplitter(graphics, false,
			                                           //descriptionTip,
			                                           descriptionTip3,
			                                           docSpacer
			                                           );*/

			var mainSplitter = new TipSplitter(graphics, true,
				countMessageTip,
				countSpacer,
				descSplitter);

			sections.Insert(0, mainSplitter);
			var mainSplitter2 = new TipSplitter(graphics, false,
				sections.ToArray());

			// Show it.
			var size = TipPainter.GetTipSize(control, graphics, mainSplitter2);
			DrawingRectangle1 = countMessageTip.DrawingRectangle1;
			DrawingRectangle2 = countMessageTip.DrawingRectangle2;
			return size;
		}

		return Size.Empty;
	}

	public static Size DrawHelpTip(Control control,
		Graphics graphics, Font font,
		string countMessage,
		string basicDescription,
		string documentation,
		int beg_bold, int bold_len, int param_num, bool addit_info)
	{
		if (IsVisibleText(countMessage) ||
		    IsVisibleText(basicDescription) ||
		    IsVisibleText(documentation))
		{
			// Create all the TipSection objects.
			var countMessageTip = new CountTipText(graphics, font, countMessage);

			var countSpacer = new TipSpacer(graphics, new SizeF(IsVisibleText(countMessage) ? 4 : 0, 0));

			var descriptionTip = new TipText(graphics, font, basicDescription);
			descriptionTip.insight_wnd = control is PABCNETInsightWindow;
			descriptionTip.beg_bold = beg_bold;
			descriptionTip.len_bold = bold_len;
			var descriptionTip1 = new TipText(graphics, font, basicDescription.Substring(0, beg_bold));
			var descriptionTip2 = new TipText(graphics, new Font(font, FontStyle.Bold), basicDescription.Substring(beg_bold, bold_len));
			var descriptionTip3 = new TipText(graphics, font, basicDescription.Substring(beg_bold + bold_len));
			descriptionTip.desc1 = descriptionTip1;
			descriptionTip.desc2 = descriptionTip2;
			descriptionTip.desc3 = descriptionTip3;
			var docSpacer = new TipSpacer(graphics, new SizeF(0, IsVisibleText(documentation) ? 4 : 0));
			var sections = new List<TipSection>();
			if (documentation != null)
				try
				{
					var params_ind = documentation.IndexOf("<params>");
					TipText paramsTextTipHeader = null;
					TipText paramsTextTip = null;
					if (params_ind != -1 && addit_info)
					{
						paramsTextTipHeader = new TipText(graphics, new Font(font, FontStyle.Bold), StringResources.Get("CODE_COMPLETION_PARAMETERS"));
						paramsTextTipHeader.is_head = true;
						var end_param = documentation.IndexOf("<returns>", params_ind);
						string params_text;
						if (end_param == -1)
							params_text = documentation.Substring(params_ind + 8).Trim(' ', '\n', '\r', '\t');
						//paramsTextTip = new TipText(graphics,font,params_text);
						else
							params_text = documentation.Substring(params_ind + 8, end_param - params_ind - 8).Trim(' ', '\n', '\r', '\t');
						//paramsTextTip = new TipText(graphics,font,documentation.Substring(params_ind+8,end_param-params_ind-8).Trim(' ','\n','\r','\t'));
						var prms_list = new List<string>();
						var ind = params_text.IndexOf("<param>", 0);
						while (ind != -1)
						{
							var end_ind = params_text.IndexOf("</param>", ind + 1);
							prms_list.Add(params_text.Substring(ind + 7, end_ind - ind - 7).Trim(' ', '\n', '\t', '\r'));
							ind = params_text.IndexOf("<param>", end_ind);
							if (ind != -1)
							{
								if (ind - end_ind - 7 > 0)
									prms_list.Add(params_text.Substring(end_ind + 8, ind - end_ind - 8).Trim(' ', '\n', '\t', '\r'));
								else
									prms_list.Add("");
							}
							else
							{
								prms_list.Add(params_text.Substring(end_ind + 8).Trim(' ', '\n', '\t', '\r'));
							}
						}

						var prm_sections = new List<TipSection>();
						var i = 0;
						while (i < prms_list.Count)
						{
							var prm_name_sect = new TipText(graphics, font, prms_list[i]);
							prm_name_sect.param_name = true;
							var desc_param_sect = new TipText(graphics, font, prms_list[i + 1]);
							desc_param_sect.need_tab = true;
							desc_param_sect.param_desc = true;
							//TipSplitter spltr = new TipSplitter(graphics,true,prm_name_sect,desc_param_sect);
							if (!string.IsNullOrEmpty(prms_list[i + 1]))
							{
								prm_sections.Add(prm_name_sect);
								prm_sections.Add(desc_param_sect);
							}

							i += 2;
						}

						if (prm_sections.Count > 0)
						{
							sections.Add(new TipText(graphics, new Font(font.FontFamily, 3), " "));
							sections.Add(paramsTextTipHeader);
							sections.AddRange(prm_sections);
						}
					}

					var return_ind = documentation.IndexOf("<returns>");
					/*TipText returnTextTipHeader = null;
					TipText returnTextTip = null;
					if (return_ind != -1 && addit_info)
					{
					    returnTextTipHeader = new TipText(graphics,new Font(font, FontStyle.Bold),PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN"));
					    returnTextTipHeader.is_head = true;
					    int end_return = documentation.IndexOf("<params>",return_ind);
					    string return_text;
					    if (end_return == -1)
					        return_text = documentation.Substring(return_ind+9).Trim(' ','\n','\r','\t');
					    else
					        return_text = documentation.Substring(return_ind+9,end_return-return_ind-9).Trim(' ','\n','\r','\t');
				
					    if (!string.IsNullOrEmpty(return_text))
					    {
					        returnTextTip = new TipText(graphics,font,return_text);
					        sections.Add(new TipText(graphics,new Font(font.FontFamily,3)," "));
					        sections.Add(returnTextTipHeader);
					        sections.Add(returnTextTip);
					        returnTextTip.need_tab = true;
					    }
					}*/
					if (params_ind != -1 && return_ind != -1)
						documentation = documentation.Substring(0, Math.Min(params_ind, return_ind)).Trim(' ', '\n', '\t', '\r');
					else if (params_ind != -1)
						documentation = documentation.Substring(0, params_ind).Trim(' ', '\n', '\t', '\r');
					else if (return_ind != -1)
						documentation = documentation.Substring(0, return_ind).Trim(' ', '\n', '\t', '\r');
				}
				catch
				{
				}

			var docTip = new TipText(graphics, font, documentation);
			//docTip.is_doc = true;
			docTip.need_tab = true;
			sections.Insert(0, docTip);
			if (!string.IsNullOrEmpty(documentation))
			{
				var descr_head = new TipText(graphics, new Font(font, FontStyle.Bold), StringResources.Get("CODE_COMPLETION_DESCRIPTION"));
				descr_head.is_head = true;
				sections.Insert(0, descr_head);
				sections.Insert(0, new TipText(graphics, new Font(font.FontFamily, 1), " "));
			}

			var descSplitter = new TipSplitter(graphics, true,
				descriptionTip,
				docSpacer
			);
			// Now put them together.
			/*TipSplitter descSplitter = new TipSplitter(graphics,true,
			                                           descriptionTip1,
			                                           descriptionTip2,
			                                           descriptionTip3,
			                                           docSpacer
			                                           );*/
			descSplitter.is_desc = true;
			/*TipSplitter descSplitter1 = new TipSplitter(graphics, false,
			                                           //descriptionTip,
			                                           descriptionTip1
			                                           );
			
			TipSplitter descSplitter2 = new TipSplitter(graphics, false,
			                                           //descriptionTip,
			                                           descriptionTip2
			                                           );
			
			TipSplitter descSplitter3 = new TipSplitter(graphics, false,
			                                           //descriptionTip,
			                                           descriptionTip3,
			                                           docSpacer
			                                           );*/

			var mainSplitter = new TipSplitter(graphics, true,
				countMessageTip,
				countSpacer,
				descSplitter);

			sections.Insert(0, mainSplitter);
			var mainSplitter2 = new TipSplitter(graphics, false,
				sections.ToArray());
			// Show it.
			var size = TipPainter.DrawTip(control, graphics, mainSplitter2);
			DrawingRectangle1 = countMessageTip.DrawingRectangle1;
			DrawingRectangle2 = countMessageTip.DrawingRectangle2;
			return size;
		}

		return Size.Empty;
	}

	internal static bool IsVisibleText(string text)
	{
		return text != null && text.Length > 0;
	}
}