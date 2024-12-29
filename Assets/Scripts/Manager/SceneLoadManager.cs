using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using DG.Tweening;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference menu;
    public CanvasGroup transitionCanvasGroup;

    public bool isFade;
    public float fadeDuration = 1.0f; // 渐变时间

    [Header(header:"事件广播")]
    public ObjectEventSO AfterLevelLoadedEvent;
    public ObjectEventSO BeforeLevelLoadEvent;

    public ObjectEventSO UpdateLevelEvent;

    public ObjectEventSO BeforeScceneUnLoadEvent;
    public ObjectEventSO AfterSceneLoadEvent;


    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public async void OnSelectLevelEvent(object data)
    {
        LevelDataSO currentData = data as LevelDataSO;
        currentScene = currentData.sceneToLoad;

        //BeforeLevelLoadEvent.RaiseEvent(this, this);
        // 淡出
        await FadeOut();

        //卸载场景
        await UnLoadSceneTask();

        //加载场景
        await LoadSceneTask();

        // 淡入
        await FadeIn();

        AfterLevelLoadedEvent.RaiseEvent(currentData, this);
    }

    private async Awaitable LoadSceneTask()
    {

        var s  = currentScene.LoadSceneAsync(LoadSceneMode.Additive);

        await s.Task;
        if(s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }

    }

    private async Awaitable UnLoadSceneTask()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    // 淡出：透明 -> 不透明
    public async Task FadeOut()
    {
        if (transitionCanvasGroup == null)
        {
            Debug.LogError("Transition CanvasGroup 未设置，无法执行淡出");
            return;
        }

        transitionCanvasGroup.gameObject.SetActive(true); // 确保 CanvasGroup 激活
        await transitionCanvasGroup.DOFade(1f, fadeDuration).AsyncWaitForCompletion(); // DOTween 异步等待
    }

    // 淡入：不透明 -> 透明
    public async Task FadeIn()
    {
        if (transitionCanvasGroup == null)
        {
            Debug.LogError("Transition CanvasGroup 未设置，无法执行淡入");
            return;
        }

        await transitionCanvasGroup.DOFade(0f, fadeDuration).AsyncWaitForCompletion(); // DOTween 异步等待
        transitionCanvasGroup.gameObject.SetActive(false); // 淡入完成后隐藏
    }

    /// <summary>
    /// 监听返回地图的事件函数
    /// </summary>

    public async void LoadMap()
    {
        await UnLoadSceneTask();

        currentScene = map;

        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if(currentScene != null)
            await UnLoadSceneTask();
    
        currentScene = menu;

        await LoadSceneTask();
    }


}
