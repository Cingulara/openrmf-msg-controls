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