using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class AlembicController : MonoBehaviour
{
    private AlembicStreamPlayer alembicPlayer;
    private float loopDuration;

    void Start()
    {
        alembicPlayer = GetComponent<AlembicStreamPlayer>();
        alembicPlayer.CurrentTime = alembicPlayer.StartTime;
        loopDuration = alembicPlayer.EndTime - alembicPlayer.StartTime;
    }

    void Update()
    {
        float timeInLoop = Time.time % loopDuration;
        alembicPlayer.CurrentTime = alembicPlayer.StartTime + timeInLoop;

        alembicPlayer.UpdateImmediately(alembicPlayer.CurrentTime);
    }
}
