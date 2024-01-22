// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;

using MS.Internal;
using MS.Win32;

namespace System.Windows.Automation.Peers
{
    // work around (ItemsControl.GroupStyle doesn't show items in groups in the UIAutomation tree)
    // this class should be public
    internal class ItemsControlWrapperAutomationPeer : ItemsControlAutomationPeer
    {
        public ItemsControlWrapperAutomationPeer(ItemsControl owner)
            : base(owner)
        { }

        override protected ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            if (item is UIElement element)
            {
                // Some UIElements have their own automation peers, so we need to check for that.
                var peer = element.CreateAutomationPeer();
                if (peer is not null)
                {
                    return new ItemsControlElementAutomationPeer(element, peer, this);
                }

                // Some other UIElements don't have their own automation peers, so we treat them as ItemsControlItems.
            }

            // If the item is not a UIElement, or if it is a UIElement that doesn't have its own automation peer,
            // we create an ItemsControlItemAutomationPeer for it.
            return new ItemsControlItemAutomationPeer(item, this);
        }

        override protected string GetClassNameCore()
        {
            return "ItemsControl";
        }

        override protected AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.List;
        }
    }
}

