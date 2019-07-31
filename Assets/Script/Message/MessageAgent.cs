using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageAgent : MonoBehaviour
{

    [SerializeField,Header("UI")] Text _message;
    [SerializeField, Header("Config"),Range(0f,10f)] float _durtime = 2f;

    private bool _show = false;
    private Queue<MessageDataItem> messageQueue;
    private bool _queueItemRun = false;

    IEnumerator _currentShow;

    void Start()
    {
    }

    void Update()
    {

        if (!_queueItemRun)
        {
            if (messageQueue.Count > 0)
            {
                _queueItemRun = true;
                var agent = messageQueue.Dequeue();

                string message = agent.Message;
                float durtime = agent.Durtime;
                Show(message, durtime);
            }
            else {
                // 不存在消息队列
                Close();
            }
        }
        else {
            // 此时消息队列中有消息正在显示中

        }
        //print(imgRtf.localPosition);

    }


    public void UpdateMessage(string message,float durtime)
    {
        CheckQueue();
        StopCurrentShow();
        var messageItem = new MessageDataItem(message,durtime);
        messageQueue.Enqueue(messageItem);
        Open();

    }

    public void UpdateMessage(string message)
    {
        CheckQueue();
        StopCurrentShow();
        var messageItem = new MessageDataItem(message, 1000f);
        messageQueue.Enqueue(messageItem);
        Open();
    }

    private void Open() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);

        }

    }

    private void CheckQueue() {
        if (messageQueue == null)
        {
            messageQueue = new Queue<MessageDataItem>();
            _queueItemRun = false;
        }
    }


    public void Close()
    {
        StopCurrentShow();

        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
            _show = false;

        }
    }

    private void Show(string content,float durtime) {
        if (!_show) {
            gameObject.SetActive(true);
            _show = true;
        }

        _message.text = content;
        _currentShow = ShowMessage(durtime);
        StartCoroutine(_currentShow);
    }


    IEnumerator ShowMessage(float durtime)
    {

        yield return new WaitForSeconds(durtime);

        _queueItemRun = false;
    }

    private void StopCurrentShow() {
        if (_currentShow != null) {
            StopCoroutine(_currentShow);
            _currentShow = null;
            _queueItemRun = false;
        }

    }


}


/// <summary>
///     消息队列中使用
/// </summary>
public class MessageDataItem {
    string _message;
    float _durtime;

    public MessageDataItem(string message,float durtime) {
        _message = message;
        _durtime = durtime;
    }

    public string Message { get => _message; set => _message = value; }
    public float Durtime { get => _durtime; set => _durtime = value; }
}
