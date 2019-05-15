using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class BusinessCardAgent : MonoBehaviour
{
    int _index; // 当前索引，从0开始

    List<BusinessCardCellAgent> pool;
    CardAgent _cardAgent;

    bool _doingNext = false;
    bool _doingReturn = false;


    /// <summary>
    ///  Component
    /// </summary>
    [SerializeField] BusinessCardCellAgent _cellPrefab;
    [SerializeField] RectTransform _content;
    [SerializeField] Button _btnClose;
    [SerializeField] Button _btnReturn;
    [SerializeField] Button _btnNext;


    void Update()
    {
        
    }

    public void Init(CardAgent cardAgent) {
        _cardAgent = cardAgent;
        UpdateContents();
        UpdateToolStatus();
    }


    public void DoClickNext()
    {
        if (!_doingNext) {
            _doingNext = true;

            pool[_index + 1].GoFront(() => {
                pool[_index].GoBackLeft();
                _index++;
                UpdateToolStatus();

                _doingNext = false;
            });
        }
    }

    public void DoClickClose() {
        _cardAgent.CloseBusinessCard();
    }

    public void DoClickReturn()
    {
        if (!_doingReturn)
        {
            _doingReturn = true;
            pool[_index - 1].GoFront(() =>
            {
                pool[_index].GoBackRight();
                _index--;
                UpdateToolStatus();

                _doingReturn = false;
            });
        }
    }


    public void UpdateContents() {
        if (pool == null) {
            pool = new List<BusinessCardCellAgent>();
        }

        // 根据id获取 business card 内容
        List<Texture> EnvCardsTexures = DaoService.Instance.GetEnvCards(_cardAgent.Id);

        for (int i = 0; i < EnvCardsTexures.Count; i++) {
            Texture texture = EnvCardsTexures[i];

            //创建card
            BusinessCardCellAgent businessCardCellAgent = Instantiate(
                            _cellPrefab,
                            _content
                            ) as BusinessCardCellAgent;

            BusinessCardData businessCardData = new BusinessCardData();
            businessCardData.Index = i;
            businessCardData.Image = texture;
            businessCardCellAgent.UpdateContent(businessCardData);

            pool.Add(businessCardCellAgent);
        }

    }


    //  更新工具信息
    void UpdateToolStatus() {

        // 如果内容只有一张，则只显示xx按钮
        if (pool.Count == 1)
        {
            _btnReturn.gameObject.SetActive(false);
            _btnNext.gameObject.SetActive(false);
        }
        else
        {
            // 如果 Index 为 0 ，则不显示回退按钮
            if (_index == 0)
            {
                _btnReturn.gameObject.SetActive(false);
                if (!_btnNext.gameObject.activeSelf)
                {
                    _btnNext.gameObject.SetActive(true);
                }
            }
            else if (_index == pool.Count - 1)
            {
                _btnNext.gameObject.SetActive(false);
                if (!_btnReturn.gameObject.activeSelf)
                {
                    _btnReturn.gameObject.SetActive(true);
                }
            }
            else {
                if (!_btnReturn.gameObject.activeSelf)
                {
                    _btnReturn.gameObject.SetActive(true);
                }
            }
        }
    }


}


