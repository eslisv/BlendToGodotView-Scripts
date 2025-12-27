using Godot;

namespace AM.ModelViewerTool
{
    public static class TweenUtil
    {
        public static Tween TweenYRotation(Node parent, float radians, float duration, bool isRelative)
        {
            Tween rotateTween = parent.CreateTween();
            PropertyTweener propertyTweener = rotateTween.TweenProperty(parent, "rotation:y", radians, duration);
            if (isRelative)
                propertyTweener.AsRelative();
            return rotateTween;
        }
    }
}