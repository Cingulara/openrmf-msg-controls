// Copyright (c) Cingulara 2019. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.
using Microsoft.EntityFrameworkCore;
using openrmf_msg_controls.Models;

namespace openrmf_msg_controls.Database
{
    public class ControlsDBContext : DbContext  
    {  
        public ControlsDBContext(DbContextOptions<ControlsDBContext> context): base(context)  
        {  
    
        }  

        public DbSet<ControlSet> ControlSets { get; set; }
    }  
}