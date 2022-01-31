using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GleasonUsers.Model
{
    public class FlyoutMenuItem
    {
        public FlyoutMenuItem()
        {
            TargetType = typeof(FlyoutMenuItem);
        }

        public int Id { get; set; }
        public Type TargetType { get; set; }
        public string Title { get; set; }
    }   
}