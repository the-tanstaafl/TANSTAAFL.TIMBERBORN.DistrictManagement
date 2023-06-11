﻿using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.UiBuilderSystem;
using Timberborn.Core;
using Timberborn.CoreUI;
using Timberborn.Navigation;
using UnityEngine;
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
                            .SetWidth(new Length(900, Pixel))
                            .SetHeight(new Length(400, Pixel))
                            .AddPreset(factory => factory.Labels()
                                .GameTextBig(name: "BeaverArmslengthLabel",
                                        text: $"Beaver Arms length: {DistrictManagement._savedConfig.BeaverArmsLength}",
                                        builder: labelBuilder => labelBuilder
                                            .SetStyle(style => style.alignSelf = Align.Center)))
                            .AddPreset(factory => factory.Sliders()
                                .SliderIntCircle(1, 100, DistrictManagement._savedConfig.BeaverArmsLength,
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
                                .SliderIntCircle(1, 100, (int)DistrictManagement._savedConfig.ResourceBuildingsRange,
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
                                .SliderIntCircle(1, 100, DistrictManagement._savedConfig.BuildersRange,
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

            var builder = _dialogBoxShower.Create()
                .AddContent(_root)
                .SetConfirmButton(UpdateConfigs, "Save")
                .SetCancelButton(() => { }, "Cancel");

            builder.Show();

            var size = new Vector2(1000, 500);
            var size2 = new Vector2(950, 450);
            var panel = builder._panelStack.TopPanel.VisualElement;
            panel.SetSize(size);
            foreach (var child in panel.Children())
            {
                foreach (var grandchild in child.Children())
                {
                    grandchild.SetSize(size);

                    foreach (var grandgrandchild in grandchild.Children())
                    {
                        grandgrandchild.SetSize(size2);
                    }
                }
            }

            //var buttons = panel.Q("Buttons");
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
