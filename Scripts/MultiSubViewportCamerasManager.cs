using EVLibrary.Godot;
using EVLibrary.Godot.XML;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using SysFile = System.IO.File;

namespace AM.ModelViewerTool
{
    // I recommend changing this so that you make classes for each thing being written to xml like have a camera xml camera class and a vector3 xml class that camera xml class uses and then have a extension function (or regular function) for exporting it to an xml string and vice versa creating an instance from a xml string. And a class for everything you're writing here that holds a list of the camera xml data.
    public sealed partial class MultiSubViewportCamerasManager : Node
    {
        private const string ROOT_ELEMENT_NAME = "Root";

        public Action AddViewportCallback;
        public Action<int> CloseViewportCallback;

        public void Setup(Action addViewportCallback, Action<int> closeViewportCallback)
        {
            AddViewportCallback = addViewportCallback;
            CloseViewportCallback = closeViewportCallback;

            if (!Directory.Exists(PathReferences.GetAbsolutePathToCameraLayoutPathFolder()))
            {
                Directory.CreateDirectory(PathReferences.GetAbsolutePathToCameraLayoutPathFolder());
            }
        }

        public void SavePreset(string filePath)
        {
            XElement xmlRoot = new XElement(ROOT_ELEMENT_NAME);
            int index = 0;
            foreach (IMultiSubViewportCameraController camera in MultiSubViewportCamerasRegistration.Cameras)
            {
                Node3D cameraNode = camera.Node3D;
                xmlRoot.Add(
                    new Camera3DXml(cameraNode).Element
                );
                ++index;
            }
            SysFile.WriteAllText(filePath, xmlRoot.ToString());
        }

        public void LoadPreset(string layoutPath)
        {
            if (!SysFile.Exists(layoutPath))
            {
                return;
            }
            SettingsController.Settings.CameraLayoutFilePath = layoutPath;
            XDocument doc = XDocument.Load(layoutPath);
            // Grabs all pairs of position and rotation vectors. Ex: { Camera 1 Pos, Camera 1 Rot, ... , Camera n Pos, Camera n Rot }
            string xPathExpression = string.Format(
                "//{0} | //{1}",
                Camera3DXml.POSITION_ELEMENT_NAME,
                Camera3DXml.ROTATION_ELEMENT_NAME
            );
            List<XElement> vector3Elements = doc.XPathSelectElements(xPathExpression).ToList();

            IEnumerable<IMultiSubViewportCameraController> viewportCameras = MultiSubViewportCamerasRegistration.Cameras;
            int cameraCount = (int)(vector3Elements.Count * 0.5);
            while (viewportCameras.Count() < cameraCount)
            {
                AddViewportCallback.Invoke();
                GD.Print("Adding camera");
            }
            while (viewportCameras.Count() > cameraCount)
            {
                CloseViewportCallback.Invoke(viewportCameras.Count() - 1);
                GD.Print("Removing camera");
            }

            int viewportIndex = 0;
            for (int i = 0; i + 1 < vector3Elements.Count(); i += 2)
            {
                Vector3Xml positionElem = vector3Elements[i].ConvertToVector3XML();
                Vector3Xml rotationElem = vector3Elements[i + 1].ConvertToVector3XML();
                GodotPrintHelper.PrintVariable(nameof(positionElem) + nameof(positionElem.Vector), positionElem.Vector);
                GodotPrintHelper.PrintVariable(nameof(rotationElem) + nameof(rotationElem.Vector), rotationElem.Vector);

                Node3D cameraNode = viewportCameras.ElementAt(viewportIndex).Node3D;
                cameraNode.Position = positionElem.Vector;
                cameraNode.Rotation = rotationElem.Vector;
                ++viewportIndex;
            }
        }
    }
}