using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Hades_Map_Editor.PropertiesSection
{
    public interface IPropertyItem: IComponent
    {
        Control GetPanel();
        Control GetTitle();
    }
    public enum PropertyType
    {
        activateAtRange,
        activationRange,
        active,
        allowMovementReaction,
        ambient,
        angle,
        attachToID,
        attachedIDs,
        causesOcculsion,
        clutter,
        collision,
        color,
        comments,
        createsShadows,
        dataType,
        drawVfxOnTop,
        flipHorizontal,
        flipVertical,
        groupNames,
        helpTextId,
        hue,
        id,
        ignoreGridManager,
        invert,
        location,
        name,
        offsetZ,
        parallaxAmount,
        points,
        saturation,
        scale,
        skewAngle,
        skewScale,
        sortIndex,
        stopsLight,
        tallness,
        useBoundsForSortArea,
        value
    }
}
