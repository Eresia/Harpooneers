using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollidable {

    void OnCollision(Vector3 velocity);
}
