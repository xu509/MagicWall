using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDataSource
{
    private static TheDataSource instance;

    private TheDataSource() {

    }

    public static TheDataSource GetInstance() {
        if (instance == null) {
            instance = new TheDataSource();
        }
        return instance;
    }




}
