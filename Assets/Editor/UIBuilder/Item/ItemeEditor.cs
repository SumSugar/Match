using Codice.Client.BaseCommands.Merge.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

public class ItemeEditor : EditorWindow
{
    private ItemDataListSO ItemDataBase;
    private List<ItemDetails> ItemList;
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private VisualTreeAsset itemRowTemplate;
    private ListView itemListView;

    private Sprite defaultIcon;

    private ScrollView itemDetailsSection;
    private ItemDetails activeItem;
    private VisualElement iconPreview;

    [MenuItem("Inventory/ItemeEditor")]
    public static void ShowExample()
    {
        ItemeEditor wnd = GetWindow<ItemeEditor>();
        wnd.titleContent = new GUIContent("ItemeEditor");

        // 限制最小和最大窗口大小
        wnd.minSize = new Vector2(800, 600); // 最小宽度和高度

    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        //加载模版
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UIBuilder/Item/ItemRowTemplate.uxml");

        //默认Icon
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Addons/Synty/InterfaceFantasyWarriorHUD/Sprites/Icons_Map/ICON_FantasyWarrior_Map_Unknown01_Clean.png");

        //获得按键
        root.Q<Button>("AddItem").clicked += OnAddItemClick;
        root.Q<Button>("RemoveItem").clicked += OnRemoveItemClick;

        //变量赋值
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");

        //加载数据
        LoadDataBase();

        GenerateListView();
    }

    #region 按钮事件
    private void OnRemoveItemClick()
    {
        ItemList.Remove(activeItem);

        itemListView.ClearSelection();
        itemListView.Rebuild();

        itemDetailsSection.visible = false;
    }

    private void OnAddItemClick()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "New Item";
        newItem.itemID = 1000 + ItemList.Count;
        ItemList.Add(newItem);


        itemListView.Rebuild();
    }
    #endregion

    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataListSO");
        if(dataArray.Length > 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            ItemDataBase = AssetDatabase.LoadAssetAtPath<ItemDataListSO>(path);
        }

        ItemList = ItemDataBase.itemDetailsList; 
        EditorUtility.SetDirty(ItemDataBase);
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if(i < ItemList.Count)
            {
                e.Q<VisualElement>("Icon").style.backgroundImage = ItemList[i].itemIcon == null ? defaultIcon.texture : ItemList[i].itemIcon.texture;

                e.Q<Label>("Name").text = ItemList[i].itemName == null? "No ITEM" : ItemList[i].itemName;
            }
        };
        itemListView.fixedItemHeight = 60;
        itemListView.itemsSource = ItemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        itemListView.selectionChanged += OnListSelectionChange;

        itemDetailsSection.visible = false;
    }

    private void OnListSelectionChange(IEnumerable<object> selectionItem)
    {
        if (selectionItem.Count() > 0)
        {
            activeItem = (ItemDetails)selectionItem.First();
            GetItemDetails();
            itemDetailsSection.visible = true;
        }
    }

    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();

        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild();
        });

        //其他所有变量的绑定
        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldSprite;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnWorldSprite = (Sprite)evt.newValue;
        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;
        });

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemBaseValue").value = activeItem.itemBaseValue;
        itemDetailsSection.Q<IntegerField>("ItemBaseValue").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemBaseValue = evt.newValue;
        });


        itemDetailsSection.Q<IntegerField>("ItemRare").value = activeItem.itemRare;
        itemDetailsSection.Q<IntegerField>("ItemRare").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemRare = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemPrice").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("ItemPrice").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });

    }
}
