﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.Web.Infrastructure.Helpers
{
	public static class CustomHelpers
	{
		public static MvcHtmlString CreateCheckbox(this HtmlHelper helper, string propertyName, string name, List<string> selectedItems)
		{
			var div = new TagBuilder("div");

			if (selectedItems.Contains(name))
			{
				div.InnerHtml += $"<p><label for=\"{name}\"><input type='checkbox' id=\"{name}\" name=\"{propertyName}\" value=\"{name}\" checked /> " + name + "</label></p>";
			}
			else
			{
				div.InnerHtml += $"<p><label for=\"{name}\"><input type='checkbox' id=\"{name}\" name=\"{propertyName}\" value=\"{name}\" /> " + name + "</label></p>";
			}

			return new MvcHtmlString(div.InnerHtml);
		}

		public static MvcHtmlString CreateRadioButton(this HtmlHelper helper, string propertyName, string name, string selectedItem)
		{
			var div = new TagBuilder("div");

			if (name == selectedItem)
			{
				div.InnerHtml += $"<p><label for=\"{name}\"><input type='radio' id=\"{name}\" name=\"{propertyName}\" value=\"{name}\" checked /> " + name + "</label></p>";
			}
			else
			{
				div.InnerHtml += $"<p><label for=\"{name}\"><input type='radio' id=\"{name}\" name=\"{propertyName}\" value=\"{name}\" /> " + name + "</label></p>";
			}

			return new MvcHtmlString(div.InnerHtml);
		}

		public static MvcHtmlString CreateRadioButtonGroup(this HtmlHelper helper, string propertyName, Enum value)
		{
			var div = new TagBuilder("div");

			foreach (var option in Enum.GetNames(value.GetType()))
			{
				if (option == value.ToString())
				{
					div.InnerHtml += $"<p><label for={option}><input type='radio' id={option} name={propertyName} value={option} checked /> " + option + "</label></p>";
				}
				else
				{
					div.InnerHtml += $"<p><label for={option}><input type='radio' id={option} name={propertyName} value={option} /> " + option + "</label></p>";
				}
			}

			return new MvcHtmlString(div.ToString());
		}
	}
}