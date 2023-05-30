﻿using Microsoft.EntityFrameworkCore;

namespace Conflict.Server.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }
	}
}