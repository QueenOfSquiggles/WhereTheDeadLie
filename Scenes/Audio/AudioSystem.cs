using bridge;

public partial class AudioSystem : Node
{

	[Export] private NodePath path_wailing_sfx;
	[Export] private NodePath path_wind_sfx;
	private AudioStreamPlayer sfx_wailing;
	private AudioStreamPlayer sfx_wind;

	private readonly Random random = new();

	public override void _Ready()
	{
		sfx_wailing = this.GetNodeCustom<AudioStreamPlayer>(path_wailing_sfx);
		sfx_wind = this.GetNodeCustom<AudioStreamPlayer>(path_wind_sfx);
	}

	public void PlayWailing() => sfx_wailing.Play();

	public void PlayWind() => sfx_wind.Play();

	public void PlayWailingChance(float percent_chance)
	{
		if(random.NextSingle() > percent_chance) return;
		PlayWailing();
	}
	public void PlayWindChance(float percent_chance)
	{
		if(random.NextSingle() > percent_chance) return;
		PlayWind();
	}

}