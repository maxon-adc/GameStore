﻿using GameStore.Authentification.Abstract;
using GameStore.Common.Abstract;
using GameStore.Common.Entities;
using GameStore.DAL.Abstract.Common;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace GameStore.Authentification.Concrete
{
	public class Authentication : IAuthentication
	{
		private const string CookieName = "__GAMESTORE_AUTH";

		private readonly IUserRepository _repository;
		private readonly IHashGenerator<string> _hashGenerator;

		private IPrincipal _currentUser;

		public Authentication(IUserRepository repository,
			IHashGenerator<string> hashGenerator)
		{
			_repository = repository;
			_hashGenerator = hashGenerator;
		}

		public HttpContextBase HttpContext { get; set; }
		public User User => ((IUserProvider)CurrentUser.Identity).User;

		public User LogIn(string login, string password, bool isPersistent)
		{
			var hashedPassword = _hashGenerator.Generate(password);
			var user = _repository.GetSingleOrDefault(u => u.Login == login && u.Password == hashedPassword && u.IsDeleted == false);

			if (user != null)
			{
				var cookie = CreateCookie(login, isPersistent);
				HttpContext.Response.Cookies.Set(cookie);
			}

			return user;
		}

		public void LogOut()
		{
			var cookie = HttpContext.Response.Cookies[CookieName];

			if (cookie != null)
			{
				cookie.Value = string.Empty;
			}
		}

		public IPrincipal CurrentUser
		{
			get
			{
				if (_currentUser != null)
				{
					return _currentUser;
				}

				var cookie = HttpContext.Request.Cookies.Get(CookieName);

				if (!string.IsNullOrEmpty(cookie?.Value))
				{
					var ticket = FormsAuthentication.Decrypt(cookie.Value);
					_currentUser = new UserProvider(ticket?.Name, _repository);
				}
				else
				{
					_currentUser = new UserProvider();
				}

				return _currentUser;
			}
		}

		private HttpCookie CreateCookie(string login, bool isPersistent = false)
		{
			var encryptedTicket = CreateTicket(login, isPersistent);
			var cookie = new HttpCookie(CookieName)
			{
				Value = encryptedTicket,
				Expires = DateTime.UtcNow.Add(FormsAuthentication.Timeout)
			};

			return cookie;
		}

		private string CreateTicket(string login, bool isPersistent = false)
		{
			var ticket = new FormsAuthenticationTicket(1, login, DateTime.UtcNow,
				DateTime.UtcNow.Add(FormsAuthentication.Timeout), isPersistent, string.Empty, FormsAuthentication.FormsCookiePath);
			var encryptedTicket = FormsAuthentication.Encrypt(ticket);

			return encryptedTicket;
		}
	}
}