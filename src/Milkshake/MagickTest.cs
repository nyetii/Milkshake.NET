using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake
{
    public class MagickTest
    {
        private MilkshakeService _service;

        public MagickTest(MilkshakeService service)
        {
            _service = service;
        }

        public async Task Test(string ab)
        {
            using var image =
                new MagickImage(@"K:\_BCK\Milkshake Simulator\images\" + ab);

            IMedia a = new Source();

            a.Id = "43242";
            a.Name = "test";
            a.Path = "a";
            a.Height = image.Height;
            a.Width = image.Width;

            await image.WriteAsync(@"C:\Users\Netty\Desktop\test.png");

            await _service.LogAsync("it works?", Severity.Warning);

        }
    }
}
