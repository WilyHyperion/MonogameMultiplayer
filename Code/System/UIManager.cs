
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Abstract.UI;
namespace Game.GameSystem.UI;
public static class UIManager {
    public static List<UIElement> Elements = new List<UIElement>();
    public static void Draw(SpriteBatch s){
        foreach (UIElement e in Elements){
            if(e.Visible)
                e.Draw(s);
        }
    }
    public static void UpdateElements(){
        foreach (UIElement e in Elements){
            e.UpdateElement();
        }
    }
    public static void loadElementTextures(){
        var allElements = typeof(UIElement).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(UIElement)));
        foreach (var element in allElements){
            try{
                var texture = UnamedGame.Instance.Content.Load<Texture2D>("UI/" + element.Name);
                var method = element.GetMethod("LoadTexture", BindingFlags.Static | BindingFlags.Public |  BindingFlags.FlattenHierarchy, null, new Type[] {typeof(Texture2D)}, null);
                method.Invoke(null, new object[] {texture});
            }
            catch (Exception e){
                Console.WriteLine("Error loading texture for " + element.Name);
            }

        }
    }
    public static void AddElement(UIElement e){
        Elements.Add(e);
    }
    public static void RemoveElement(UIElement e){
        Elements.Remove(e);
    }
}