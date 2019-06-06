using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsFactoryAgent : MonoBehaviour
{
    [SerializeField] private EnvFactory _envFactory;
    [SerializeField] private ActivityFactory _activityFactory;
    [SerializeField] private ProductFactory _productFactory;

    MagicWallManager _manager;

    void Awake() {
    }

    public void Init(MagicWallManager manager) {
        _manager = manager;

        _envFactory.Init(_manager);
        _activityFactory.Init(_manager);
        _productFactory.Init(_manager);

    }



    public EnvFactory envFactory{get { return _envFactory; } }

    public ActivityFactory activityFactory { get { return _activityFactory; } }

    public ProductFactory productFactory { get { return _productFactory; } }

}
