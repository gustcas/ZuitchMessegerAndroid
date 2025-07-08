using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager2.Widget;
using System;
using WoWonder.Adapters;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.Tab.Fragment
{
    public class TabChatFragment : AndroidX.Fragment.App.Fragment
    {
        #region Variables Basic

        private ViewPager2 ViewPager;
        private MainTabAdapter TabAdapter;
        private LinearLayout Tab;

        // Botones y textos de las pestañas
        private LinearLayout ButtonHome, ButtonChats, ButtonGroups, ButtonArchives;
        private TextView    TxtHome,    TxtChats,  TxtGroups,  TxtArchives;

        // Fragmentos
        public HomeFragment            HomeTab;
        public LastChatFragment        LastChatTab;
        public LastGroupChatsFragment  LastGroupChatsTab;
        public ArchivedChatsFragment   ArchivedChatsTab;

        #endregion

        #region General

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                return inflater.Inflate(Resource.Layout.TabChatLayout, container, false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            InitComponent(view);
            AddFragmentsTabs();
        }

        #endregion

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                TabAdapter = new MainTabAdapter(this);

                ViewPager = view.FindViewById<ViewPager2>(Resource.Id.viewPagerChat);
                Tab       = view.FindViewById<LinearLayout>(Resource.Id.tabsChat);

                // Asignar controles
                ButtonHome     = view.FindViewById<LinearLayout>(Resource.Id.llHome);
                ButtonChats    = view.FindViewById<LinearLayout>(Resource.Id.llChats);
                ButtonGroups   = view.FindViewById<LinearLayout>(Resource.Id.llGroups);
                ButtonArchives = view.FindViewById<LinearLayout>(Resource.Id.llArchive);

                TxtHome     = view.FindViewById<TextView>(Resource.Id.ivHome);
                TxtChats    = view.FindViewById<TextView>(Resource.Id.ivChat);
                TxtGroups   = view.FindViewById<TextView>(Resource.Id.ivGroups);
                TxtArchives = view.FindViewById<TextView>(Resource.Id.ivArchive);

                // Eventos
                if (!ButtonHome.HasOnClickListeners)
                {
                    ButtonHome.Click     += ButtonHome_Click;
                    ButtonChats.Click    += ButtonChats_Click;
                    ButtonGroups.Click   += ButtonGroups_Click;
                    ButtonArchives.Click += ButtonArchives_Click;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void AddFragmentsTabs()
        {
            try
            {
                TabAdapter.ClaerFragment();

                HomeTab     = new HomeFragment();
                LastChatTab = new LastChatFragment();

                if (AppSettings.EnableChatGroup)
                    LastGroupChatsTab = new LastGroupChatsFragment();

                if (AppSettings.EnableChatArchive)
                    ArchivedChatsTab = new ArchivedChatsFragment();

                // Añadir fragmentos al adaptador
                TabAdapter.AddFragment(HomeTab,     GetText(Resource.String.Lbl_Home));   // 0
                TabAdapter.AddFragment(LastChatTab, GetText(Resource.String.Lbl_User));   // 1

                if (AppSettings.EnableChatGroup)
                    TabAdapter.AddFragment(LastGroupChatsTab, GetText(Resource.String.Lbl_Group)); // 2

                if (AppSettings.EnableChatArchive)
                    TabAdapter.AddFragment(ArchivedChatsTab,  GetText(Resource.String.Lbl_Archive)); // 3

                // Configurar ViewPager
                ViewPager.UserInputEnabled   = false;
                ViewPager.OffscreenPageLimit = TabAdapter.ItemCount;
                ViewPager.Orientation        = ViewPager2.OrientationHorizontal;
                ViewPager.Adapter            = TabAdapter;
                ViewPager.Adapter.NotifyDataSetChanged();

                // Abrir por defecto en «Chats» (índice 1)
                ViewPager.CurrentItem = 0;
                ResetTabs();
                ResaltarPestaña(ButtonHome, TxtHome);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// Limpia estilos de todas las pestañas.
        /// </summary>
        private void ResetTabs()
        {
            ButtonHome.SetBackgroundResource(0);
            ButtonChats.SetBackgroundResource(0);
            ButtonGroups.SetBackgroundResource(0);
            ButtonArchives.SetBackgroundResource(0);

            var normal = Resources.GetColor(Resource.Color.text_color_in_between, null);
            TxtHome.SetTextColor(normal);
            TxtChats.SetTextColor(normal);
            TxtGroups.SetTextColor(normal);
            TxtArchives.SetTextColor(normal);
        }

        /// <summary>
        /// Aplica estilo seleccionado a la pestaña activa.
        /// </summary>
        private void ResaltarPestaña(LinearLayout btn, TextView txt)
        {
            btn.SetBackgroundResource(Resource.Drawable.bg_tab_button);
            txt.SetTextColor(Color.White);
        }

        #endregion

        #region Event

        private void ButtonHome_Click(object sender, EventArgs e)
        {
            ResetTabs();
            ResaltarPestaña(ButtonHome, TxtHome);
            ViewPager.SetCurrentItem(0, false);
        }

        private void ButtonChats_Click(object sender, EventArgs e)
        {
            ResetTabs();
            ResaltarPestaña(ButtonChats, TxtChats);
            ViewPager.SetCurrentItem(1, false);
        }

        private void ButtonGroups_Click(object sender, EventArgs e)
        {
            ResetTabs();
            ResaltarPestaña(ButtonGroups, TxtGroups);
            ViewPager.SetCurrentItem(2, false);
        }

        private void ButtonArchives_Click(object sender, EventArgs e)
        {
            ResetTabs();
            ResaltarPestaña(ButtonArchives, TxtArchives);
            ViewPager.SetCurrentItem(3, false);
        }

        #endregion
    }
}
