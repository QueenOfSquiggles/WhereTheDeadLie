namespace bridge;

public static class NodeExtensions {

    public static T GetNodeCustom<T>(this Node node, NodePath path) where T : class
    {
        var value = node.GetNodeOrNull<T>(path);
        if (value == null)
        {
            var msg = $"Failed to acquire node from provided path! {node}:\t'{path}'";
            GD.PrintErr(msg);
            GD.PushError(msg);
            return null;
        }
        return value;
    }
/*
    public static T GetNodeCustom<T>(this player_character node, NodePath path) where T : class 
    {
        return node.GetNodeCustom<T>(path);
    }
*/


}