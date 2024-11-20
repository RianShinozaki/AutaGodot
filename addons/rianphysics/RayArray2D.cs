using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class RayArray2D : Node2D {
    public int arrayNum;
    public Godot.Collections.Array<RayCast2D> arrays;
    public float maxAngle = 46;
    [Export] int floorOffset = 16;

    public bool IsCollidingCheckAngle(RayCast2D ray) {
        if(!ray.IsColliding()) return false;
        if(maxAngle == -1) 
            return true;
        else {
            float theAngle = ray.TargetPosition.Normalized().AngleTo(-ray.GetCollisionNormal());
            theAngle = Mathf.RadToDeg(theAngle);
            return Mathf.Abs(theAngle) < maxAngle;
        }   
    }
    public bool IsColliding() {
        for (int i = 0; i < arrayNum; i++){ 
            if (IsCollidingCheckAngle(arrays[i])) return true;
        }
        return false;
    }
    
    public float getCollidingRatio() {
        int colls = 0;
        for (int i = 0; i < arrayNum; i++) {
            if (IsCollidingCheckAngle(arrays[i])) colls++;
        }
        return colls / arrayNum;
    }

    public Vector2 GetCollisionPoint() {
        Vector2 result = Vector2.Zero;
        int colls = 0;
        List<int> missingRays = new List<int>();
        for (int i = 0; i < arrayNum; i++) {
            if (IsCollidingCheckAngle(arrays[i])) {
                colls++;
                result += arrays[i].GetCollisionPoint();
            } else {
                missingRays.Add(i);
            }
        }
        //If we're missing colliders, create fake ones
        if(colls < arrayNum) {
            Vector2 floorN = GetCollisionNormal();
            foreach(int i in missingRays) {
                Vector2 offset = arrays[i].Position * floorN;
                Vector2 fakePos = arrays[i].GlobalPosition + new Vector2(offset.Y, offset.X) - Vector2.Up*floorOffset;

                result += fakePos;
                colls++;
            }
        }
        return result / colls;
    }
    public Vector2 GetCollisionNormal() {
        Vector2 result = Vector2.Zero;
        int colls = 0;
        for (int i = 0; i < arrayNum; i++) {
            if (IsCollidingCheckAngle(arrays[i])) {
                colls++;
                result += arrays[i].GetCollisionNormal();
            }
        }
        return result / colls;
    }

    public GodotObject GetCollider() {
        return arrays[1].GetCollider();
    }

    public Vector2 TargetPosition {
        get {
            Vector2 result = Vector2.Zero;
            for (int i = 0; i < arrayNum; i++) {
                result += arrays[i].TargetPosition;
            }
            return result / arrayNum;
        }
        set {
            for (int i = 0; i < arrayNum; i++) {
                arrays[i].TargetPosition = value;
            }
        }
    }

    public Vector2 GlobalOrigin{
        get {
            Vector2 result = Vector2.Zero;
            for (int i = 0; i < arrayNum; i++) {
                result += arrays[i].GlobalTransform.Origin;
            }
            return result / arrayNum;
        }
    }

    public override void _Ready() {
        base._Ready();
        arrayNum = GetChildCount();
        arrays = new Godot.Collections.Array<RayCast2D>();
        for (int i = 0; i < arrayNum; i++) {
            arrays.Add((RayCast2D)GetChild(i));
        }
    }
}