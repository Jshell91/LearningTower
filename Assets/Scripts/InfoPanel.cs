using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public TMP_Text grade, cluster, desc;

    // When opening the infopanel, we update its text values.
    public void Open(Block block) {
        gameObject.SetActive(true);
        grade.text = block.grade + ": " + block.domain;
        cluster.text = block.cluster;
        desc.text = block.standarddescription;
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
