using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.UiBuilderSystem;
using Timberborn.Core;
using Timberborn.CoreUI;
using Timberborn.Navigation;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace TANSTAAFL.TIMBERBORN.DistrictManagement
{
    public class ConfigBox
    {
        public static Action OpenOptionsDelegate;
        private readonly DialogBoxShower _dialogBoxShower;
        private readonly UIBuilder _builder;
        private NavigationDistance _navigationDistance;

        private VisualElement _root;

        public ConfigBox(DialogBoxShower dialogBoxShower, UIBuilder builder, NavigationDistance navigationDistance)
        {
            OpenOptionsDelegate = OpenOptionsPanel;
            _dialogBoxShower = dialogBoxShower;
            _builder = builder;
            _navigationDistance = navigationDistance;
        }

        private void OpenOptionsPanel()
        {
            _root = _builder.CreateComponentBuilder()
                            .CreateVisualElement()
                            .AddPreset(factory => factory.Labels()
                                .GameTextBig(name: "BeaverArmslengthLabel",
                                        text: $"Beaver Arms length: {DistrictManagement._savedConfig.BeaverArmsLength}",
                                        builder: labelBuilder => labelBuilder
                                            .SetStyle(style => style.alignSelf = Align.Center)))
                            .AddPreset(factory => factory.Sliders()
                                .SliderIntCircle(1, 50, DistrictManagement._savedConfig.BeaverArmsLength,
                                        name: "BeaverArmslengthSlider",
                                        builder: sliderBuilder => sliderBuilder
                                            .SetStyle(style => style.flexGrow = 1f)
                                            .SetPadding(new Padding(new Length(21, Pixel), 0))))

                            .AddPreset(factory => factory.Labels()
                                .GameTextBig(name: "ResourceBuildingsLabel",
                                        text: $"Resource Buildings range: {DistrictManagement._savedConfig.ResourceBuildingsRange}",
                                        builder: labelBuilder => labelBuilder
                                            .SetStyle(style => style.alignSelf = Align.Center)))
                            .AddPreset(factory => factory.Sliders()
                                .SliderIntCircle(1, 50, (int)DistrictManagement._savedConfig.ResourceBuildingsRange,
                                        name: "ResourceBuildingsSlider",
                                        builder: sliderBuilder => sliderBuilder
                                            .SetStyle(style => style.flexGrow = 1f)
                                            .SetPadding(new Padding(new Length(21, Pixel), 0))))

                            .AddPreset(factory => factory.Labels()
                                .GameTextBig(name: "BuildersLabel",
                                        text: $"Builders range: {DistrictManagement._savedConfig.BuildersRange}",
                                        builder: labelBuilder => labelBuilder
                                            .SetStyle(style => style.alignSelf = Align.Center)))
                            .AddPreset(factory => factory.Sliders()
                                .SliderIntCircle(1, 50, DistrictManagement._savedConfig.BuildersRange,
                                        name: "BuildersSlider",
                                        builder: sliderBuilder => sliderBuilder
                                            .SetStyle(style => style.flexGrow = 1f)
                                            .SetPadding(new Padding(new Length(21, Pixel), 0))))
                            .BuildAndInitialize();

            var beaverLabel = _root.Q<Label>("BeaverArmslengthLabel");
            var beaverSlider = _root.Q<SliderInt>("BeaverArmslengthSlider");
            beaverSlider.RegisterValueChangedCallback(x => beaverLabel.text = $"Beaver Arms length: {beaverSlider.value}");

            var resourceBuildingsLabel = _root.Q<Label>("ResourceBuildingsLabel");
            var resourceBuildingsSlider = _root.Q<SliderInt>("ResourceBuildingsSlider");
            resourceBuildingsSlider.RegisterValueChangedCallback(x => resourceBuildingsLabel.text = $"Resource Buildings range: {resourceBuildingsSlider.value}");

            var buildersLabel = _root.Q<Label>("BuildersLabel");
            var buildersSlider = _root.Q<SliderInt>("BuildersSlider");
            buildersSlider.RegisterValueChangedCallback(x => buildersLabel.text = $"Builders range: {buildersSlider.value}");

            _dialogBoxShower.Create()
                .AddContent(_root)
                .SetConfirmButton(UpdateConfigs)
                .SetCancelButton(() => { })
                .Show();
        }

        private void UpdateConfigs()
        {
            DistrictManagement._savedConfig.BeaverArmsLength = _root.Q<SliderInt>("BeaverArmslengthSlider").value;
            DistrictManagement._savedConfig.ResourceBuildingsRange = _root.Q<SliderInt>("ResourceBuildingsSlider").value;
            DistrictManagement._savedConfig.BuildersRange = _root.Q<SliderInt>("BuildersSlider").value;

            DistrictManagement.ApplyConfigs(_navigationDistance);
        }
    }
}
