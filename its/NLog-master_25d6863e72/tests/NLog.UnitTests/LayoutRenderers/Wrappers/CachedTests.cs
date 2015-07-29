// 
// Copyright (c) 2004-2011 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

namespace NLog.UnitTests.LayoutRenderers.Wrappers
{
    using NLog;
    using NLog.Layouts;
    using Xunit;

    public class CachedTests : NLogTestBase
    {
        [Fact]
        public void CachedLayoutRendererWrapper()
        {
            SimpleLayout l = "${guid}";
            
            string s1 = l.Render(LogEventInfo.CreateNullEvent());
            string s2 = l.Render(LogEventInfo.CreateNullEvent());
            string s3;

            // normally GUIDs are never the same
            Assert.NotEqual(s1, s2);

            // but when you apply ${cached}, the guid will only be generated once
            l = "${cached:${guid}:cached=true}";
            s1 = l.Render(LogEventInfo.CreateNullEvent());
            s2 = l.Render(LogEventInfo.CreateNullEvent());
            Assert.Equal(s1, s2);

            // calling Close() on Layout Renderer will reset the cached value
            l.Renderers[0].Close();
            s3 = l.Render(LogEventInfo.CreateNullEvent());
            Assert.NotEqual(s2, s3);

            // calling Initialize() on Layout Renderer will reset the cached value
            l.Renderers[0].Close();
            string s4 = l.Render(LogEventInfo.CreateNullEvent());
            Assert.NotEqual(s3, s4);

            // another way to achieve the same thing is using cached=true
            l = "${guid:cached=true}";
            s1 = l.Render(LogEventInfo.CreateNullEvent());
            s2 = l.Render(LogEventInfo.CreateNullEvent());
            Assert.Equal(s1, s2);

            // another way to achieve the same thing is using cached=true
            l = "${guid:cached=false}";
            s1 = l.Render(LogEventInfo.CreateNullEvent());
            s2 = l.Render(LogEventInfo.CreateNullEvent());
            Assert.NotEqual(s1, s2);
        }
    }
}