﻿/*
Copyright (c) 2014, Lars Brubaker
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using System.Threading;
using MatterHackers.Agg.Platform;
using MatterHackers.MatterControl.Tests.Automation;
using MatterHackers.PolygonMesh;
using MatterHackers.PolygonMesh.Csg;
using MatterHackers.PolygonMesh.Processors;
using MatterHackers.VectorMath;
using NUnit.Framework;

namespace MatterHackers.MatterControl.Slicing.Tests
{
	[TestFixture, Category("MatterControl.Slicing")]
	public class SliceLayersTests
	{
		//[Test]
		public void SliceLayersGeneratingCorrectSegments()
		{
			// TODO: Make tests work on Mac as well as Windows
			if (AggContext.OperatingSystem == OSType.Mac)
			{
				return;
			}

			string meshFileName = TestContext.CurrentContext.ResolveProjectPath(4, "Tests", "TestData", "TestMeshes", "SliceLayers", "Box20x20x10.stl");
			Mesh cubeMesh = StlProcessing.Load(meshFileName, CancellationToken.None);

			AxisAlignedBoundingBox bounds = cubeMesh.GetAxisAlignedBoundingBox();
			Assert.IsTrue(bounds.ZSize == 10);


			//var alllayers = slicelayers.getperimetersforalllayers(cubemesh, .2, .2);
			//assert.istrue(alllayers.count == 50);

			//foreach (slicelayer layer in alllayers)
			//{
			//	assert.istrue(layer.unorderedsegments.count == 8);

			//	// work in progress
			//	//assert.istrue(layer.perimeters.count == 1);
			//	//assert.istrue(layer.perimeters[0].count == 8);
			//}

			//alllayers = slicelayers.getperimetersforalllayers(cubemesh, .2, .1);
			//Assert.IsTrue(allLayers.Count == 99);
		}
	}
}