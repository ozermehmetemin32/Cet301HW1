using System;
using Microsoft.Maui.Controls;

namespace CalculatorCet;

public partial class MainPage : ContentPage
{
    private enum Islem { Yok, Topla, Cikar, Carp, Bol }

    private double _ilkSayi;
    private Islem _secilenIslem;

    public MainPage()
    {
        InitializeComponent();
    }
    
    private bool SayiAl(out double sayi)
    {
        sayi = 0;
        string metin = (InputEntry?.Text ?? "").Trim().Replace(',', '.');
        
        if (string.IsNullOrWhiteSpace(metin))
            return false;

        return double.TryParse(
            metin,
            System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture,
            out sayi
        );
    }
    
    private void IslemeTiklaninca(Islem islem)
    {
        if (!SayiAl(out double sayi))
        {
            sonucLabel.Text = "Sonuc: Once bir sayi girin";
            return;
        }

        _ilkSayi = sayi;
        _secilenIslem = islem;
        
        InputEntry.Text = string.Empty;
        sonucLabel.Text = "Sonuc:";
    }

    private void ToplamayaTiklaninca(object sender, EventArgs e) 
        => IslemeTiklaninca(Islem.Topla);

    private void CikarmayaTiklaninca(object sender, EventArgs e) 
        => IslemeTiklaninca(Islem.Cikar);

    private void CarpTiklaninca(object sender, EventArgs e) 
        => IslemeTiklaninca(Islem.Carp);

    private void BolTiklaninca(object sender, EventArgs e) 
        => IslemeTiklaninca(Islem.Bol);
    
    private void EsittirTiklaninca(object sender, EventArgs e)
    {
        if (_secilenIslem == Islem.Yok)
        {
            if (SayiAl(out double sayi))
                sonucLabel.Text = $"Sonuc: {sayi}";
            else
                sonucLabel.Text = "Sonuc: Bir sayi girin";
            return;
        }
        
        if (!SayiAl(out double ikinciSayi))
        {
            sonucLabel.Text = "Sonuc: Ikinci sayiyi girin";
            return;
        }
        
        double sonuc = 0;
        bool basarili = true;

        switch (_secilenIslem)
        {
            case Islem.Topla:
                sonuc = _ilkSayi + ikinciSayi;
                break;
            case Islem.Cikar:
                sonuc = _ilkSayi - ikinciSayi;
                break;
            case Islem.Carp:
                sonuc = _ilkSayi * ikinciSayi;
                break;
            case Islem.Bol:
                if (Math.Abs(ikinciSayi) < 0.0000000001)
                {
                    sonucLabel.Text = "Sonuc: Sifira bolunemez";
                    basarili = false;
                }
                else
                {
                    sonuc = _ilkSayi / ikinciSayi;
                }
                break;
        }

        if (basarili)
        {
            sonucLabel.Text = $"Sonuc: {sonuc}";
            _ilkSayi = sonuc;
            _secilenIslem = Islem.Yok;
            InputEntry.Text = string.Empty;
        }
    }
}
