﻿using System.Collections.Generic;

namespace GameStore.Web.Models
{
	public class AllCommentsViewModel
	{
		public AllCommentsViewModel()
		{
			Comments = new List<CommentViewModel>();
		}

		public string GameId { get; set; }

		public string GameKey { get; set; }

		public List<CommentViewModel> Comments { get; set; }
	}
}