using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.HowToPlay;

namespace Terraria
{
	public sealed class TextSequenceBlock
	{

#if !USE_ORIGINAL_CODE
		private static int Width = 450;
#else
		private const int Width = 450;
#endif

		private const int DisplayTime = 480;

		public static readonly string[] TIPS_EN = new string[78]
		{
			"You can change your spawn point by placing and using a <i>bed</i>.",
			"If you find a <i>Magic Mirror</i>, you can use it to teleport back to your spawn point.",
			"<i>Torches</i> require <i>wood</i> and <i>gels</i> to craft. Gels can be found on slimes.",
			"When you explore caves, it helps to have <i>wood platforms</i>. Craft them out of wood.",
			"<i>Falling Stars</i> sometimes appear at night. Collect 10 of them to craft a  <i>Mana Crystal</i> you can use to increase your <i>mana</i>.",
			"You can plant <i>acorns</i> to grow new trees.",
			"Use the <i>Housing</i> section of the Inventory Menu to assign NPCs to rooms or to toggle NPC room flags.",
			"You can check if a room is <i>valid</i> housing from the <i>Housing</i> section of the Inventory Menu.",
			"If you get lost or need to find another player, open the <i>World Map</i>.",
			"You can remove furniture or background walls by crafting a <i>hammer</i>.",
			"There are <i>floating islands</i> in the sky.",
			"Sometimes you can find NPCs hidden around the World.",
			"During a <i>Blood Moon</i>, zombies can open doors.",
			"You can continuously use some items by holding down " + Lang.RightTrigger + ".",
			"Water will break your fall.",
			"<i>Torches</i> and <i>glowsticks</i> can be a light for you in dark places, when all other lights go out. Torches won't work underwater, but glowsticks will.",
			"Don't fall into lava without an <i>Obsidian Skin Potion</i>!",
			"You won't take falling damage if you have a <i>Lucky Horseshoe</i>. Look for them on floating islands.",
			"Walking on Hellstone and Meteorite can burn you! Protect yourself by equipping an <i>Obsidian Skull</i> or similar accessory.",
			"<i>Life Crystals</i> are hidden around the World. Use them to increase your health.",
			"Some ores require better pickaxes to mine.",
			"Bosses are easier to defeat with friends.",
			"Press " + Lang.RightStick + " to switch between Cursor Modes.",
			"Bows and guns require the proper <i>ammo</i> in your Ammo slots.",
			"If your Inventory is full, you can press " + Lang.XButton + " to send items to the Trash.",
			"When speaking to a vendor, you can sell items in your Inventory by pressing " + Lang.XButton + ".",
			"The <i>Old Man</i> at the Dungeon is a <i>Clothier</i>, if only someone could lift his <i>curse</i>...",
			"Collect money to attract a <i>Merchant</i> to your house.",
			"Hold onto an explosive to attract a <i>Demolitionist</i> to your house.",
			"Make sure you have empty, valid rooms to attract new inhabitants.",
			"Slay a boss to attract a <i>Dryad</i> to your house. She can tell you the state of <i>Corruption</i> and <i>Hallow</i> in your World.",
			"Bind items to your " + Lang.DPad + " for quick access!",
			"By default, " + Lang.LeftStickPress + " is an alternate jump button. Advanced players may wish to switch the grappling and alternate jump buttons; you can do this from the Controls Menu.",
			"Wear a <i>Mining Helmet</i> if you don't want to use torches.",
			"You can wear <i>buckets</i>!",
			"You can remove torches with " + Lang.BButton + ".",
			"Other players can loot your chests! If you don't trust them, use a <i>safe</i> or <i>piggy bank</i>; those items have storage that is exclusive to each player.",
			"Defeat the boss in The Underworld to change the World forever. Find a <i>Guide Voodoo Doll</i> and hurl it into the infernal lava to summon him.",
			"<i>Demon Altars</i> can't be destroyed with a normal hammer. You have to pwn them.",
			"Killing <i>bunnies</i> is cruel. Period.",
			"<i>Explosives</i> are dangerous!\n...and effective...",
			"Watch out for falling <i>Meteorites!</i>",
			"A <i>pet</i> can be your best friend.",
			"If you dig deep enough, you'll end up in <i>The Underworld</i>!",
			"<i>Santa Claus</i> is real. He comes to town after the <i>Frost Legion</i> is defeated (and 'tis the season).",
			"Don't shake a <i>snow globe</i> unless you want to summon the <i>Frost Legion</i>.",
			"In your Inventory, you can press " + Lang.YButton + " to equip items such as armor or accessories directly to a usable slot.",
			"You can use <i>Hallowed Seeds</i>, <i>Holy Water</i>, or <i>Pearlstone</i> to make Hallowed ground.",
			"The Hallow is the only place where Corruption cannot spread.",
			"The Corruption is full of chasms. Mind the gaps.",
			"Time heals all wounds.",
			"Rocket science gave us <i>Rocket Boots</i>.",
#if (!IS_PATCHED && VERSION_INITIAL)	// It took me too long to find this typo...
			"The <i>Could in a Bottle</i> and <i>Shiny Red Balloon</i> accessories both improve your ability jump. Combine them to make a <i>Cloud in a Balloon</i>.",
#else
			"The <i>Cloud in a Bottle</i> and <i>Shiny Red Balloon</i> accessories both improve your ability jump. Combine them to make a <i>Cloud in a Balloon</i>.",
#endif
			"<i>Gungnir</i> is the spear of the god Odin.",
			"<i>Excalibur</i> is the legendary sword of King Arthur.",
			"<i>Adamantite</i> comes from the Greek word 'adamastos', meaning 'untameable'",
			"If you store your coins in a house, you'll be less likely to lose them.",
			"If you find chests out in the World, you can remove them with a <i>hammer</i> to take them with you.",
			"To craft potions, place a <i>bottle</i> on a <i>table</i> to make an <i>alchemy station</i>. Double, double, toil and trouble!",
			"If your house doesn't have background walls, monsters will be able to spawn inside.",
			"Monsters spawn more often in dark areas.",
			"Wearing armor made out of all the same material gives you an extra bonus.",
			"Build a <i>furnace</i> to craft metal bars out of ore.",
			"You can harvest <i>cobwebs</i> and turn them into <i>silk</i>. You can use silk to craft vanity clothes or a bed.",
			"You can buy <i>wires</i> from a Mechanic and use them to create traps, pumping systems, or other elaborate devices.",
			"If you're sick of getting knocked around, try equipping a <i>Cobalt Shield</i>. You can find it in the Dungeon.",
			"You can make a <i>grappling hook</i> out of <i>iron chains</i> and a <i>hook</i>. The easiest place to find a hook is on piranhas in a jungle.",
			"A room in a house can have wood platforms as a floor or ceiling, but NPCs need at least one solid block to stand on.",
			"You can destroy <i>Shadow Orbs</i> with a hammer or explosives, but prepare yourself for the forces they unleash.",
			"The most powerful items require <i>Souls</i> to craft. You can only find them in Hard Mode.",
			"When dealing with a <i>Goblin Army</i>, crowd control is key.",
			"The best wizard people use <i>Mana Flowers</i>.",
			"Use <i>suspicious looking</i> items at your own risk!",
			"Sand is overpowered.",
			"Swimming is dangerous without <i>Flippers</i> or a <i>Diving Helmet</i>! You can craft those with a <i>Tinkerer's Workshop</i>.",
			"The <i>Goblin Tinkerer</i> found in underground caverns will sell you many useful items, including a <i>Tinkerer's Workshop</i>.",
			"<i>Seeds</i> can be used to farm a variety of useful ingredients, especially for crafting potions.",
			"Some projectiles such as <i>arrows</i> and <i>shurikens</i> can be gathered and reused after firing them."
		};

		public static readonly string[] TIPS_DE = new string[78]
		{
			"Du kannst deinen Startpunkt ändern, indem du ein <I>Bett</I> platzierst und benutzt.",
			"Wenn du einen <I>Magischen Spiegel</I> findest, kannst du dich damit zu deinem Startpunkt zurückteleportieren.",
			"Um <I>Fackeln</I> herzustellen, benötigst du <I>Holz</I> und <I>Glibber</I>. Glibber können bei Schleimen gefunden werden.",
			"Wenn du Höhlen erforschst, ist es hilfreich, <I>Holzklappen</I> zu haben.  Stelle sie aus Holz her.",
			"<I>Gefallene Sterne</I> erscheinen manchmal in der Nacht. Sammle zehn davon, um einen <I>Manakristall</I> herzustellen. Damit kannst du dein <I>Mana</I> erhöhen.",
			"Du kannst Eicheln pflanzen, damit neue Bäume wachsen.",
			"Du kannst die <I>Unterkunft</I>-Sektion des Inventars nutzen, um den Zimmern NPCs zuzuweisen oder zwischen den NPC-Raumflaggen zu wechseln.",
			"In der Unterkunfts-Sektion des Inventarmenüs kannst du prüfen, ob ein Zimmer eine <I>fertige</I> <I>Unterkunft</I> ist.",
			"Wenn du die Orientierung verlierst oder einen anderen Spieler finden willst, öffne die <I>Weltkarte</I>.",
			"Du kannst Möbel und Hintergrundwände entfernen, indem du einen <I>Hammer</I> herstellst.",
			"Es gibt <I>schwebende Inseln</I> in der Luft.",
			"Manchmal kannst du versteckte NPCs finden, die in der Welt verstreut sind.",
			"Während eines <I>Blutmondes</I> können Zombies die Türen öffnen.",
			"Manche Gegenstände kannst du kontinuierlich benutzen, indem du " + Lang.RightTrigger + " gedrückt hältst.",
			"Wasser federt deinen Sturz ab.",
			"<I>Fackeln</I> und <I>Leuchtstäbe</I> können dir Licht in der Dunkelheit spenden, wenn alle anderen Lichter versagen. Fackeln funktionieren nicht unter Wasser, aber Leuchtstäbe schon.",
			"Falle nicht in die Lava, ohne einen <I>Obsidianhaut-Trank</I>!",
			"Du erleidest keinen Fallschaden, wenn du ein <I>Glückshufeisen</I> hast. Suche nach ihnen auf den schwebenden Inseln.",
			"Auf Höllensteinen und Meteoriten zu laufen, kann dich verbrennen! Schütze dich, indem du einen <I>Obsidianschädel</I> oder einen ähnlichen Gegenstand trägst.",
			"<I>Lebenskristalle</I> sind überall in der Welt versteckt. Verwende sie, um deine Gesundheit zu verbessern.",
			"Manche Erze können nur mit einer Spitzhacke abgebaut werden.",
			"Bosse sind mit der Hilfe von Freunden leichter zu besiegen.",
			"Drücke " + Lang.RightStick + " , um zwischen den Cursor Modi zu wechseln.",
			"Bögen und Pistolen benötigen die richtige <I>Munition</I> in deinen Ammo-Slots.",
			"Wenn dein Inventar voll ist, kannst du durch Drücken von " + Lang.XButton + " einen Gegenstand in den Müll werfen.",
			"Wenn du mit einem Verkäufer sprichst, kannst du Gegenstände in deinem Inventar verkaufen, indem du " + Lang.XButton + " drückst.",
			"Der <I>Greis</I> beim Verlies ist ein <I> Schneider.</I> Wenn nur jemand seinen <I>Fluch</I> aufheben könnte ...",
			"Sammle Geld, um einen <I>Händler</I> zu deinem Haus zu locken.",
			"Behalte einen Sprengsatz, um einen <I>Sprengstoffexperte</I> zu deinem Haus zu locken.",
			"Stelle sicher, dass du leere, fertige Räume hast, bevor du neue Bewohner anziehst.",
			"Töte einen Boss, um eine <I>Dryade</I> zu deinem Haus zu locken. Sie kann dir sagen, wie <I>Heilig</I> oder <I>Verdorben</I> deine Welt ist.",
			"Lege für einen schnelleren Zugriff Gegenstände auf dein " + Lang.DPad + " !",
			"Standardmäßig ist " + Lang.LeftStickPress + " ein alternativer Knopf zum Springen. Fortgeschrittene Spieler möchten vielleicht die Knöpfe für Festhaken und Springen austauschen. Dies ist im Steuerungsmenü möglich.",
			"Trage einen <I>Minenhelm</I>, wenn du keine Fackeln benutzen möchtest.",
			"Du kannst <I>Eimer</I> tragen!",
			"Du kannst Fackeln mit " + Lang.BButton + " entfernen.",
			"Andere Spieler können deine Schatzkisten plündern! Wenn du ihnen nicht traust, benutze einen <I>Tresor</I> oder ein <I>Sparschwein</I>. Auf diese Lagergegenstände hat nur der jeweilige Besitzer Zugriff.",
			"Besiege den Boss in der Unterwelt, um die Welt für immer zu verändern. Finde eine <I>Guide Voodoo Puppe</I> und wirf sie in die höllische Lava, um ihn herbeizurufen.",
			"<I>Dämonenaltare</I> können nicht mit einem normalen Hammer zerstört werden. Du musst sie vernichtend besiegen.",
			"<I>Häschen</I> zu töten ist grausam. Punkt.",
			"<I>Sprengstoffe</I> sind gefährlich!\n ... und effektiv ... ",
			"Pass auf die herabfallenden <I>Meteoriten</I> auf!",
			"Ein <I>Haustier</I> kann dein bester Freund sein.",
			"Wenn du tief genug gräbst, landest du in der <I>Unterwelt</I>!",
			"Der <I>Weihnachtsmann</I> ist echt. Er kommt in die Stadt, nachdem die <I>Frost Legion</I> besiegt ist (und wenn es weihnachtet).",
			"Schüttle niemals eine <I>Schneekugel</I>, es sei denn, du willst die <I>Frost Legion</I> herbeirufen.",
			"In deinem Inventar kannst du " + Lang.YButton + " drücken, um Gegenstände wie Rüstung und Zubehör direkt einem offenen Slot zuzuweisen.",
			"Du kannst <I>Heilige Saat</I>, <I>Heiliges Wasser</I> oder <I>Perlstein</I> benutzen, um gesegneten Boden herzustellen.",
			"Das Heiligtum ist der einzige Platz, wo sich die Verderbtheit nicht ausbreiten kann.",
			"Die Verderbtheit ist voller Abgründe. Pass auf die Spalten auf.",
			"Die Zeit heilt alle Wunden.",
			"Die Raketentechnik hat uns <I>Raketenstiefel</I> gebracht.",
			"Die Zubehörgegenstände <I>Wolke in einer Flasche</I> und <I>Glitzender Roter Ballon</I> können beide deine Sprungkraft verbessern. Kombiniere sie, um eine <I>Wolke in einem Ballon</I> zu schaffen.",
			"<I>Gungnir</I> ist der Speer des Gottes Odin.",
			"<I>Excalibur</I> ist das legendäre Schwert des König Artus.",
			"<I>Adamantit</I> kommt vom griechischen Wort Adamastos, was unzähmbar bedeutet.",
			"Wenn du deine Münzen in einem Haus lagerst, läufst du weniger Gefahr, sie zu verlieren.",
			"Wenn du in der Welt Kisten findest, kannst du sie mit einem <I>Hammer</I> loslösen und mitnehmen.",
			"Um Tränke herzustellen, platziere eine <I>Flasche</I> auf einem <I>Tisch</I>, um eine <I> Alchemiestation</I> zu schaffen. Doppelt plagt euch, mengt und mischt!",
			"Wenn dein Haus keine Hintergrundwände hat, können Monster darin erscheinen.",
			"Monster erscheinen oft in dunklen Gebieten.",
			"Wenn du ein komplettes Rüstungs-Set aus demselben Material trägst, erhältst du einen Extra-Bonus.",
			"Baue einen <I>Schmelzofen</I>, um aus Erz Metallbarren herzustellen.",
			"Du kannst <I>Spinnweben</I> sammeln und sie zu <I>Seide</I> verarbeiten. Du kannst aus Seide Zierklamotten oder ein Bett herstellen.",
			"Du kannst <I>Kabel</I> von einem Mechaniker kaufen und aus ihnen Fallen, Pumpensysteme oder andere ausgeklügelte Geräte herstellen.",
			"Wenn du nicht mehr weggestoßen werden willst, versuche einen <I>Kobaltschild</I> zu tragen. Du kannst ihn im Verlies finden.",
			"Du kannst einen <I>Greifhaken</I> aus <I>Eisenketten</I> und einem <I>Haken</I> herstellen. Am leichtesten findest du einen Haken bei den Piranhas in einem Dschungel.",
			"Ein Zimmer in einem Haus kann Holzklappen als Fußboden oder Decke haben, aber NPCs benötigen zumindest einen festen Block, auf dem sie stehen können.",
			"Du kannst <I>Schattenkugeln</I> mit einem  Hammer oder Sprengstoff zerstören, aber bereite dich auf die Kräfte vor, die sie entfesseln.",
			"Die mächtigsten Gegenstände benötigen <I>Seelen</I> zur Herstellung. Du kannst sie nur im Schweren Modus finden.",
			"Wenn du es mit einer <I>Goblin Armee</I> zu tun hast, ist die Kontrolle der Menge der Schlüssel zum Erfolg.",
			"Die besten Magier verwenden <I>Manablumen</I>.",
			"Benutze <I>verdächtig aussehende</I> Gegenstände auf eigenes Risiko!",
			"Sand ist überwältigt.",
			"Schwimmen ist gefährlich ohne <I>Flossen</I> oder einen <I>Taucherhelm</I>! Du kannst sie in der <I>Tüftler-Werkstatt</I> herstellen.",
			"Der <I>Goblin Tüftler</I> kann in unterirdischen Höhlen gefunden werden und wird dir viele nützliche Gegenstände verkaufen, wie etwa eine <I>Tüftler-Werkstatt</I>.",
			"<I>Samen</I> können benutzt werden, um eine Vielzahl nützlicher Bestandteile anzubauen, besonders zur Herstellung von Tränken.",
			"Manche Geschosse, wie etwa <I>Pfeile</I> und <I>Shuriken</I>, können gesammelt und wiederverwendet werden, nachdem sie abgefeuert wurden."
		};

		public static readonly string[] TIPS_IT = new string[78]
		{
			"Modifica il punto di rigenerazione posizionando e utilizzando il <i>letto</i>.",
			"Se trovi uno<i> Specchio Magico</i>, usalo per teletrasportarti al punto di rigenerazione.",
			"Per creare le <i>torce</i> è necessario avere la <i>legna</i> e <i>gelatine.</i> Le gelatine si trovano sugli Slime.",
			"Quando esplori le caverne, è utile avere delle <i>piattaforme di legno</i>. Creale utilizzando la legna!",
			"Di sera a volte si vedono le <i>Stelle cadenti </i>. Raccogline 10 per creare un <i>Cristallo di Mana</i> da utilizzare per aumentare il tuo livello di<i> mana</i>.",
			"È possibile piantare <i>ghiande</i> per far crescere nuovi alberi.",
			"Utilizza la sezione <i>Alloggio</i> del menu Inventario per assegnare una stanza a un PNG o per attivare le bandiere della stanza del PNG.",
			"È possibile controllare la <i>validità</i> dell'alloggio nella sezione <i>Alloggio</i> del menu Inventario.",
			"Apri la <i>Mappa del Mondo</i> se ti perdi o devi trovare un altro giocatore.",
			"Creando un <i>martello</i> è possibile rimuovere mobili o muri sullo sfondo.",
			"Ci sono <i>isole fluttuanti</i> nel cielo.",
			"A volte è possibile trovare PNG nascosti nel Mondo.",
			"Quando c'è la <i>Luna di Sangue</i>, gli zombie possono aprire le porte.",
			"Tenendo premuto " + Lang.RightTrigger + " è possibile utilizzare continuamente alcuni oggetti.",
			"L'acqua attutirà la caduta.",
			"Le <i>torce</i> e i <i>bastoni luminosi </i>possono essere una fonte di illuminazione, quando sei in posti bui e non sono disponibili altre fonti di illuminazione. Le torce non funzionano sott'acqua, mentre i bastoni luminosi funzionano.",
			"Non cadere nella lava senza avere la <i>Pozione Pelle d'Ossidiana</i>!",
			"Se possiedi il <i>Ferro di Cavallo Fortunato</i> non ti farai male quando cadi. Cercali sulle isole fluttuanti.",
			"Puoi bruciarti se cammini sul <i>Meteorite</i> o sulla <i>Pietra Infernale</i>! Equipaggiati di un <i>Teschio d'Ossidiana </i>o di un accessorio simile per proteggerti.",
			"I <i>Cristalli di Vita</i> sono nascosti nel Mondo. Utilizzali per aumentare il tuo livello di vita.",
			"È necessario utilizzare picconi migliori per estrarre alcuni minerali.",
			"I boss sono più facili da sconfiggere con gli amici.",
			"Premi " + Lang.RightStick + " per cambiare la modalità Cursore.",
			"Archi e pistole richiedono apposite <i>munizioni </i>che troverai nelle sezioni per le munizioni.",
			"Quando l'Inventario è pieno, è possibile spostare gli oggetti nel Cestino, premendo " + Lang.XButton + ".",
			"Mentre parli con il venditore, è possibile vendere gli oggetti presenti nel tuo Inventario, premendo " + Lang.XButton + ".",
			"Il <i>Vecchio</i> nella Dungeon è un <i>Mercante di stoffe</i>... Se solo qualcuno potesse annullarne la <i>maledizione</i>...",
			"Raccogli le monete per attirare un <i>Mercante</i> nella tua abitazione.",
			"Afferra un esplosivo per attirare un <i>Esperto di demolizioni </i>nella tua abitazione.",
			"Assicurati di avere stanze valide e vuote per attirare nuovi abitanti.",
			"Uccidi un boss per attirare una <i>Driade</i> nella tua abitazione. Ti può rivelare lo stato di <i>Corruzione</i> e <i>Consacrazione </i>del tuo Mondo.",
			"Per un accesso rapido, configura gli oggetti con " + Lang.DPad + ".",
			"Per impostazione predefinita, " + Lang.LeftStickPress + " è un pulsante alternativo per saltare. I giocatori più esperti possono cambiare la modalità di lotta e alternare i pulsanti per saltare. È possibile eseguire questa azione dal menu Comandi.",
			"Se non vuoi utilizzare le torce, indossa un <i>Casco da minatore</i>.",
			"Puoi utilizzare i <i>secchi!</i>",
			"È possibile rimuovere le torce con " + Lang.BButton + ".",
			"Gli altri giocatori possono saccheggiare le tue casse! Se non ti fidi, utilizza una <i>cassaforte</i> o un <i>salvadanaio</i>. Questi sono portaoggetti accessibili solo al singolo giocatore.",
			"Sconfiggi il boss degli Inferi per cambiare per sempre il Mondo. Trova la <i>Bambola Voodoo della Guida</i> e lanciala nella lava infernale per evocarla.",
			"Gli <i>Altari dei Demoni</i> non possono essere distrutti con un martello normale. Li devi sconfiggere!",
			"È spietato uccidere i <i>conigli</i>, punto e basta!",
			"Gli <i>esplosivi</i> sono pericolosi!\n...Ed efficaci...",
			"Fai attenzione ai <i>Meteoriti</i>!",
			"Un <i>animale domestico</i> può essere il tuo migliore amico.",
			"Se scavi in profondità, finirai negli<i>Inferi</i>!",
			"<i>Babbo Natale</i> esiste! Arriva dopo aver sconfitto la <i>Legione del Gelo</i> (ed è Natale!).",
			"Non scuotere una <i>sfera di neve</i> a meno che tu non voglia evocare la <i>Legione del Gelo</i>.",
			"È possibile premere " + Lang.YButton + " nell'Inventario, per equipaggiarsi di un'armatura o un accessorio direttamente da una sezione disponibile",
			"È possibile utilizzare i <i>Semi Consacrati</i>, l'<i>Acqua Santa</i> o la <i>Pietra di Perla</i> per preparare un terreno consacrato.",
			"La Consacrazione è l'unico bioma nel quale non si espande la Corruzione.",
			"La Corruzione è piena di voragini. Fai attenzione!",
			"Il tempo guarisce tutte le ferite.",
			"La scienza missilistica ci ha donato gli <i>Stivali razzo</i>.",
			"La <i>Nuvola in Bottiglia </i>e il <i>Palloncino Rosso Brillante </i>migliorano la tua capacità di saltare. Uniscili per creare una <i>Nuvola in un Palloncino.</i>",
			"<i>Gungnir</i> è la lancia del dio Odino",
			"<i>Excalibur</i> è la leggendaria spada di Re Artù.",
			"<i>Adamantite</i> deriva dal greco 'adamastos', che significa 'indomabile'",
			"Se conservi le monete nella tua abitazione, è meno probabile che le perda.",
			"Se trovi le casse nel Mondo, potrai rimuoverle con un <i>martello</i> per portarle con te.",
			"Per creare delle pozioni, posiziona una <i>bottiglia </i>su un <i>tavolo</i> per avere una <i>postazione alchemica</i>. Abracadabra!",
			"I mostri possono rigenerarsi all'interno della tua abitazione, se non si dispone di muri sullo sfondo.",
			"I mostri si rigenerano più spesso nelle aree buie.",
			"Indossare un'armatura creata dallo stesso materiale ti dà un bonus extra.",
			"Costruisci una <i>fornace</i> per creare le barre di metallo dal ferro.",
			"È possibile raccogliere <i>ragnatele</i> e trasformarle in <i>seta</i>. È possibile utilizzare la seta per creare un letto o dei vestiti,",
			"È possibile acquistare<i> cavi</i> presso un Meccanico e utilizzarli per creare trappole, sistemi di pompaggio o altri dispositivi sofisticati.",
			"Se sei stufo di essere assalito, prova ad equipaggiarti di uno <i>Scudo di Cobalto</i>. Lo puoi trovare nel Dungeon.",
			"È possibile creare un <i>rampino</i> con delle <i>catene di ferro</i> e un <i>amo.</i> È più facile trovare un amo nella giungla, agganciato ai piranha.",
			"All'interno dell'abitazione, una stanza può avere piattaforme di legno come pavimento o soffitto, ma i PNG richiedono almeno un blocco solido su cui reggersi.",
			"È possibile distruggere Le <i>Sfere d'Ombra</i> con un martello o degli esplosivi... ma preparati alle forze che sprigioneranno.",
			"Per creare gli oggetti più potenti è necessario utilizzare le <i>Anime</i>. È possibile trovarle solo nella modalità Difficile.",
			"Quando affronti l'<i>Esercito dei Goblin</i> è importante tenere a bada la folla.",
			"I miglior stregoni utilizzano i <i>Fiori di Mana</i>.",
			"Utilizza gli oggetti per lanciare <i>sguardi sospetti</i> a tuo rischio e pericolo!",
			"La sabbia è stata conquistata.",
			"È pericoloso nuotare senza le <i>Pinne</i> o il <i>Casco da</i> <i>Sommozzatore</i>! Li puoi creare utilizzando il <i>Laboratorio dell'Inventore</i>.",
			"Il <i>Goblin Riparatore</i>, trovato nelle caverne sotterranee, ti venderà molti oggetti utili, tra cui il <i>Laboratorio dell'Inventore</i>.",
			"I <i>semi </i>possono essere utilizzati per coltivare una varietà di ingredienti utili, soprattutto per creare pozioni.",
			"Alcuni proiettili, come le <i>frecce</i> e gli <i>shuriken</i>, possono essere raccolti e riutilizzati dopo l'uso."
		};

		public static readonly string[] TIPS_FR = new string[78]
		{
			"Vous pouvez modifier votre point d'apparition en plaçant un <I>lit</I> et en l'utilisant.",
			"Si vous trouvez un <I>miroir magique</I>, vous pouvez l'utiliser pour vous téléporter à votre point d'apparition.",
			"Il faut du <I>bois</I> et du <I>gel</I> pour fabriquer les <I>torches</I>. Le gel se trouve sur les slimes.",
			"Les <I>plateformes en bois</I> sont utiles lorsque vous explorez des grottes. Fabriquez-les avec du bois.",
			"Certaines nuits, des <I>étoiles filantes</I> apparaissent. Collectez-en 10 pour fabriquer un <I>cristal mana</I> qui peut vous servir à augmenter votre <I>mana</I>.",
			"Vous pouvez planter des <I>glands</I> pour faire pousser des arbres.",
			"Utilisez la section <I>Logement</I> du menu Inventaire pour attribuer les PNJ aux chambres ou pour alterner les drapeaux des chambres de PNJ.",
			"Vous pouvez vérifier si une chambre est <I>valide</I> dans la section <I>Logement</I> du menu Inventaire.",
			"Si vous vous perdez ou que vous avez besoin de trouver un autre joueur, ouvrez la <I>Carte du monde</I>.",
			"Vous pouvez supprimer un meuble ou un mur du fond en fabriquant un <I>marteau</I>.",
			"Il y a des <I>îles flottantes</I> dans le ciel.",
			"Vous pouvez parfois trouver des PNJ cachés dans le monde.",
			"Pendant une <I>lune sanglante,</I> les zombies peuvent ouvrir les portes.",
			"Vous pouvez utiliser certains objets en continu en maintenant " + Lang.RightTrigger + ".",
			"L'eau arrêtera votre chute.",
			"Les <I>torches</I> et <I>bâtons lumineux</I> peuvent vous donner de la lumière dans les endroits sombres sans sources lumineuses. Les torches ne fonctionneront pas sous l'eau, contrairement aux bâtons lumineux.",
			"Ne tombez pas dans la lave sans une <I>potion de peau d'obsidienne</I>\u00a0!",
			"Vous ne subirez aucun dégât de chute si vous avez un <I>fer à cheval porte-bonheur</I>. Cherchez-les sur les îles flottantes.",
			"Vous pouvez brûler en marchant sur les <I>pierres de l'enfer</I> et les <I>météorites</I>\u00a0! Protégez-vous en vous équipant d'un <I>crâne d'obsidienne</I> ou d'un accessoire similaire.",
			"Des <I>cristaux de vie</I> sont dissimulés dans le monde. Utilisez-les pour augmenter votre santé.",
			"L'extraction de certains minerais nécessite une pioche plus solide.",
			"Les boss sont plus faciles à vaincre avec l'aide d'amis.",
			"Appuyez sur " + Lang.RightStick + " pour changer le mode curseur.",
			"Il vous faut des <I>munitions</I> dans vos emplacements Munitions pour vos arcs et armes à feu.",
			"Si votre inventaire est plein, vous pouvez mettre des objets à la poubelle en appuyant sur " + Lang.XButton + ".",
			"Lorsque vous parlez à un vendeur, vous pouvez vendre les objets de votre inventaire en appuyant sur " + Lang.XButton + ".",
			"Le <I>vieil homme</I> du donjon est un <I>tailleur\u00a0; s</I>i seulement quelqu'un pouvait le libérer de sa <I>malédiction</I>...",
			"Collectez de l'argent pour attirer un <I>marchand</I> chez vous.",
			"Gardez un explosif pour attirer un <I>démolisseur</I> chez vous.",
			"Veillez à avoir des chambres libres et valides pour attirer de nouveaux habitants.",
			"Éliminez un boss pour attirer une <I>dryade</I> chez vous. Elle peut vous donner l'état de <I>Corruption</I> et de <I>Sainteté</I> dans votre monde.",
			"Installez des objets sur votre " + Lang.DPad + " pour un accès rapide\u00a0!",
			"Par défaut, " + Lang.LeftStickPress + " est une alternative au bouton de saut. Les joueurs expérimentés souhaiteront peut-être changer le grappin et alterner les boutons de saut\u00a0; ceci est possible depuis le menu Commandes.",
			"Portez un <I>casque de mineur</I> si vous ne voulez pas utiliser de torches.",
			"Vous pouvez même porter des <I>seaux</I> vides\u00a0!",
			"Vous pouvez supprimer des torches avec " + Lang.BButton + ".",
			"Les autres joueurs peuvent également piller vos coffres\u00a0! Si vous ne leur faites pas confiance, utilisez un <I>coffre-fort</I> ou une <I>tirelire</I>, ces objets offrent un stockage exclusif pour chaque joueur.",
			"Vainquez le boss du monde des Enfers pour changer le monde pour toujours. Trouvez une <I>poupée vaudou du guide</I> et jetez-la dans la lave infernale pour le faire venir.",
			"Vous pouvez détruire les <I>autels du démon</I> avec une marteau normal. Vous devez les briser.",
			"Tuer des <I>lapins</I> est un acte de cruauté. Point barre.",
			"Les <I>explosifs</I> sont dangereux\u00a0!\n... Et efficaces...",
			"Faites attention aux chutes de <I>météorites</I>\u00a0!",
			"Un <I>animal de compagnie</I> peut devenir votre meilleur ami.",
			"Si vous creuser assez profondément, vous arriverez au <I>monde des Enfers</I>\u00a0!",
			"le <I>père Noël</I> existe. Il arrive en ville une fois que la Légion gel est vaincue (et c'est de saison).",
			"Ne secouez pas un globe de neige si vous ne voulez pas faire venir la Légion gel.",
			"Dans votre inventaire, vous pouvez appuyer sur " + Lang.YButton + " pour équiper les objets tels que les armures ou les accessoires dans des emplacements permettant une utilisation directe.",
			"Vous pouvez utiliser des <I>graines sanctifiées</I>, de l'<I>eau bénite</I> ou des <I>pierres de perle</I> pour créer une terre sanctifiée.",
			"La Sainteté est le seul endroit où la Corruption ne peut pas s'étendre.",
			"La Corruption est pleine d'abîmes. Attention aux trous.",
			"Le temps guérit toutes les blessures.",
			"La fuséologie nous a donné les <I>bottes-fusées.</I>",
			"Les accessoires <I>nuage en bouteille</I> et <I>ballon rouge brillant</I> permettent d'améliorer votre capacité de saut. Combinez-les pour créer un <I>nuage dans un ballon</I>.",
			"Gungnir est la lance du dieu Odin.",
			"Excalibur est l'épée légendaire du Roi Arthur.",
			"Adamantite vient du grec \u00a0'adamastos', qui signifie  'indomptable'",
			"Si vous stockez vos pièces dans une maison, vous aurez moins de chance de les perdre.",
			"Si vous trouvez des coffres dans le monde, vous pouvez les prendre à l'aide de votre <I>marteau</I> et les emmener avec vous.",
			"Fabriquez des potions en plaçant une <I>bouteille</I> sur une <I>table</I> pour créer une <I>station d'alchimie</I>. Double, double, peine et trouble\u00a0!",
			"Les monstres pourront apparaître dans votre maison si celle-ci n'a pas de mur du fond.",
			"Les monstres apparaissent plus souvent dans les zones sombres.",
			"Porter une armure assortie du même matériel vous donne un bonus.",
			"Construisez une <I>fournaise</I> pour fabriquer des lingots de métal à partir de minerai.",
			"Vous pouvez récolter des <I>toiles d'araignée</I> et en faire de la <I>soie.</I> Vous pouvez utiliser la soie pour fabriquer des vêtements de vanité ou un lit.",
			"Vous pouvez acheter des <I>câbles</I> à un mécanicien et les utiliser pour créer des pièges, des systèmes de pompage et autres dispositifs élaborés.",
			"Si vous en avez assez de prendre des coups, essayez d'équiper le <I>bouclier de cobalt</I>. Vous pouvez le trouver dans le donjon.",
			"Vous pouvez fabriquer un <I>grappin</I> avec des <I>chaînes de fer</I> et un <I>crochet.</I> Le meilleur endroit pour trouver un crochet, c'est sur les piranhas dans la jungle.",
			"Une chambre dans une maison peut avoir un plafond et un plancher faits de plateformes en bois, mais les PNJ ont besoin d'un bloc solide sur lequel se tenir.",
			"Vous pouvez détruire les <I>orbes d'ombre</I> avec un marteau ou des explosifs, mais préparez-vous pour les forces qu'elles libèrent.",
			"Des <I>âmes</I> seront nécessaires pour fabriquer les objets les plus puissants. Vous les trouverez seulement en mode difficile.",
			"Lorsque vous vous occupez d'une <I>armée de gobelins</I>, l'important c'est le contrôle de la foule.",
			"Les meilleurs sorciers utilisent des <I>fleurs de mana</I>.",
			"Utilisez les objets d'<I>apparence douteuse</I> à vos risques et périls\u00a0!'",
			"Le sable est maîtrisé.",
			"Il est dangereux de nager sans <I>palmes</I> ni <I>casque de plongée</I>\u00a0! Vous pouvez fabriquer ceux-ci à l'aide d'un <I>atelier de bricoleur</I>.",
			"Le <I>gobelin bricoleur</I> rencontré dans les grottes souterraines vous vendra des objets utiles, notamment, un <I>atelier de bricoleur</I>.",
			"Vous pouvez utiliser les graines pour cultiver une variété d'ingrédients utiles, et en particulier pour concocter des potions.",
			"Certains projectiles tels que les <I>flèches</I> ou <I>shurikens</I> peuvent être récupérés et réutilisés."
		};

		public static readonly string[] TIPS_ES = new string[78]
		{
			"Puedes cambiar el punto de resurrección colocando y usando una <I>cama</I>.",
			"Si encuentras un <I>espejo mágico</I>, puedes usarlo para teletransportarte al punto de resurrección.",
			"Para fabricar <I>antorchas</I> se necesita <I>madera</I> y <I>gel</I>. El gel se puede obtener de los slimes.",
			"Las <I>plataformas de madera</I> son muy útiles a la hora de explorar cuevas. Puedes construirlas con madera.",
			"Las <I>estrellas fugaces</I> a veces aparecen de noche. Consigue 10 de ellas para crear un <I>cristal de maná</I>que podrás usar para aumentar el <I>maná</I>.",
			"Puedes plantar <I>bellotas</I> para hacer crecer árboles.",
			"Usa la sección <I>Cobijo</I> del menú del Inventario para asignar habitaciones a los PNJ o cambiar las banderas de las habitaciones de los PNJ.",
			"Puedes comprobar si una habitación es <I>válida</I> en la sección <I>Cobijo</I> del menú del Inventario.",
			"Si te pierdes o necesitas encontrar a otro jugador, abre el <I>Mapamundi</I>.",
			"Puedes eliminar los muebles o los muros fabricando un <I>martillo</I>.",
			"En el cielo hay <I>islas flotantes</I>.",
			"A veces puedes encontrar PNJ escondidos por el mundo.",
			"Durante una <I>luna de sangre</I>, los zombies pueden abrir puertas.",
			"Puedes usar continuamente varios objetos manteniendo presionado " + Lang.RightTrigger + ".",
			"El agua detendrá tu caída.",
			"Las <I>antorchas</I> y las <I>varitas luminosas</I> se pueden usar como iluminación en sitios oscuros, si no hay otras fuentes de luz. Las antorchas no funcionan bajo el agua, pero las varitas luminosas sí.",
			"¡No te caigas en la lava sin la <I>poción de piel obsidiana</I>!",
			"No sufrirás ningún daño al caerte si tienes una <I>herradura de la suerte</I>. Búscalas en las islas flotantes.",
			"¡Caminar sobre piedras del infierno y meteoritos puede hacer que te quemes! Protégete equipándote con una <I>calavera de obsidiana</I> o un accesorio similar.",
			"Los <I>cristales de vida</I> están escondidos por el mundo. Úsalos para aumentar la salud.",
			"Algunos minerales precisan picos de mejor calidad para ser extraídos.",
			"Es más fácil derrotar a los enemigos finales con la ayuda de tus amigos.",
			"Pulsa " + Lang.RightStick + " para cambiar el modo de cursor.",
			"Los arcos y armas necesitan que coloques la <I>munición</I> adecuada en las ranuras de munición.",
			"Si tienes el inventario lleno, puedes pulsar " + Lang.XButton + " para enviar objetos a la basura.",
			"Cuando hables con un comerciante, puedes venderle objetos de tu inventario pulsando " + Lang.XButton + ".",
			"El <I>anciano<I> <I><I><I><I><I><I><I>de la mazmorra</I></I></I></I></I></I></I></I></I> es un <I>sastre</I>. Si alguien pudiese librarlo de su <I>maldición</I>...",
			"Consigue dinero para atraer a un <I>comerciante</I> a tu casa.",
			"Agárrate a un explosivo para atraer a <I>demoledores</I> a tu casa.",
			"Asegúrate de que tienes habitaciones vacías válidas para atraer a nuevos inquilinos.",
			"Descuartiza a un enemigo final para atraer a una <I>dríade </I>a tu casa. Ella te podrá informar de si tu mundo está volviéndose <I>corrompido</I> o <I>sagrado</I>.",
			"¡Enlaza objetos al " + Lang.DPad + " para acceder a ellos rápidamente!",
			"Por defecto, " + Lang.LeftStickPress + " es el botón de salto alternativo. Puede que los jugadores más experimentados prefieran cambiar los botones de agarre y salto alternativo. Esto se puede hacer desde el menú Controles.",
			"Si no quieres usar antorchas puedes llevar un <I>casco de minero</I>.",
			"¡Puedes llevar <I>cubos</I>!",
			"Puedes retirar antorchas con " + Lang.BButton + ".",
			"¡Otros jugadores pueden saquear tus cofres! Si no confías en ellos, usa una <I>caja fuerte</I> o <I>hucha</I>, ya que esos objetos tienen almacenamiento exclusivo para cada jugador.",
			"Derrota al enemigo final en el Inframundo para cambiar el mundo para siempre. Encuentra un <I>Muñeco vudú guía</I> y sumérgelo en la lava para invocarlo.",
			"Los <I>Altares demoníacos</I> no se pueden destruir con un martillo normal. Hay que destrozarlos.",
			"Matar conejitos es cruel. Punto.",
			"¡Los <I>explosivos</I> son peligrosos!\nPero efectivos...",
			"¡Cuidado con los <I>meteoritos</I>!",
			"Una <I>mascota </I>puede ser tu mejor amigo.",
			"¡Si excavas lo suficiente, terminarás en el <I>Inframundo</I>!",
			"<I>Papá Noel</I>existe. Viene a la ciudad después de haber derrotado a la <I>Legión del hielo</I>(y es la época).",
			"No agites un <I>globo de nieve</I> a menos que quieras invocar a la <I>Legión del hielo</I>.",
			"En el inventario, puedes pulsar " + Lang.YButton + " para equipar objetos como la armadura o los accesorios, moviéndolos directamente a una ranura adecuada.",
			"Puedes usar las <I>semillas encantadas</I>, el <I>agua sagrada</I> o la <I>piedra perla</I> para crear terreno sagrado.",
			"El terreno sagrado es el único sitio donde la corrupción no se puede extender.",
			"La corrupción está llena de abismos. ¡Mira bien dónde pisas!",
			"El tiempo cura todas las heridas.",
			"La ciencia nos ha proporcionado las <I>botas cohete</I>.",
			"Los accesorios, como la <I>nube en botella</I> y el <I>globo rojo brillante</I>, mejoran la habilidad de salto. Combínalos para crear una <I>nube en globo</I>.",
			"Gungnir es la lanza del dios Odín.",
			"<I>Excalibur</I>es la legendaria espada del Rey Arturo.",
			"El nombre de la <I>adamantita</I> viene de la palabra griega 'adamastos', que significa 'indomable'.",
			"Si almacenas monedas en una casa, será menos probable que las pierdas.",
			"Si encuentras cofres en el mundo, puedes eliminarlos con un <I>martillo</I> y llevártelos.",
			"Para crear pociones, coloca una <I>botella</I> sobre una <I>mesa</I>y se convertirá en una <I>estación de alquimia</I>. ¡Abracadabra, pata de cabra!",
			"Si la casa no tiene muros de fondo, los monstruos podrán entrar en ella.",
			"Los monstruos reaparecen más a menudo en las zonas oscuras.",
			"Al llevar un conjunto de armadura en el que todas las piezas estén fabricadas con el mismo material, conseguirás bonificaciones extra.",
			"Construye una <I>forja</I>para crear barras de metal a partir de los minerales.",
			"Puedes recoger las telarañas y convertirlas en <I>seda</I>. Puedes usar seda para crear camas o prendas decorativas.",
			"Puedes comprar <I>cables</I> a un mecánico y usarlos para crear trampas, sistemas de bombeo o dispositivos más elaborados.",
			"Si te cansas de que te golpeen, intenta equiparte con un <I>escudo de cobalto</I>. Lo puedes encontrar en la mazmorra.",
			"Puedes crear un <I>garfio de escalada</I> con <I>cadenas de hierro</I> y un <I>gancho</I>. Puedes obtener ganchos de las pirañas de la jungla.",
			"Una habitación puede tener plataformas de madera como suelo o techo, pero los PNJ necesitan como mínimo un bloque sólido sobre el que ponerse.",
			"Puedes destruir <I>orbes sombríos</I> con un martillo o con explosivos, pero prepárate para las fuerzas que desatan.",
			"Los objetos más poderosos precisan <I>almas</I> para ser creados. Solo podrás encontrarlas en el modo <I>Difícil</I>.",
			"Cuando te enfrentes a un <I>ejército duende</I>, es fundamental saber controlar a las multitudes.",
			"Los mejores magos usan <I>flores de maná</I>.",
			"¡Usa los objetos de <I>aspecto sospechoso</I> bajo tu responsabilidad!",
			"La arena es demasiado poderosa.",
			"¡Nadar sin <I>aletas</I> ni <I>casco de buceo</I>es peligroso! Puedes fabricar estos objetos en el <I>taller de chapuzas</I> ",
			"El <I>duende chapucero</I> se encuentra en cavernas subterráneas y te venderá muchos objetos muy útiles, como el <I>taller de chapuzas</I>.",
			"Las <I>semillas</I> se pueden usar para cultivar una gran cantidad de ingredientes muy útiles, especialmente a la hora de preparar pociones.",
			"Algunos proyectiles, como las <I>flechas</I> y los <I>shuriken</I> se pueden recoger y volver a usar después de haberlos lanzado."
		};

		private static Point TipCenter = new Point(Main.ResolutionWidth / 2, 405);

		private static Point TipDimensions = new Point(500, 150);

		private static TextBlock[] blocks;

		private int current;

		private int cooldown;

		private readonly int displayInterval;

		public static void GenerateCache(GraphicsDevice gfx)
		{
			string[] array = TIPS_EN;
			switch (Lang.LangOption)
			{
			case (int)Lang.ID.GERMAN:
				array = TIPS_DE;
				break;
			case (int)Lang.ID.FRENCH:
				array = TIPS_FR;
				break;
			case (int)Lang.ID.ITALIAN:
				array = TIPS_IT;
				break;
			case (int)Lang.ID.SPANISH:
				array = TIPS_ES;
				break;
			}

#if !USE_ORIGINAL_CODE
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					Width = 506;
					TipCenter.Y = 540;
					TipDimensions.X = (int)(TipDimensions.X * 1.15f);
					TipDimensions.Y = (int)(TipDimensions.Y * 1.15f);
					break;
				case 2:
					Width = 675;
					TipCenter.Y = 810;
					TipDimensions.X = (int)(TipDimensions.X * 1.534f);
					TipDimensions.Y = (int)(TipDimensions.Y * 1.534f);
					break;
			}
#endif

			Rectangle dialogArea = new Rectangle(TipCenter.X, TipCenter.Y, 0, 0);
			dialogArea.Inflate(TipDimensions.X >> 1, TipDimensions.Y >> 1);
			blocks = new TextBlock[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				CompiledText compiledText = new CompiledText(array[i], Width, UI.BoldSmallTextStyle);
				Rectangle textArea = new Rectangle(TipCenter.X, TipCenter.Y, 0, 0);
				textArea.Inflate(compiledText.Width >> 1, compiledText.Height + UI.BoldSmallFont.LineSpacing >> 1);

				Color TipColour = Color.DarkOrange;

#if !VERSION_FINAL && !USE_ORIGINAL_CODE
				if (Main.PSMode)
				{
					TipColour = new Color(0, 140, 255, 255); // Before 1.09 on the Old-gen PS3 version, the colour used for tips is the compliment of the X360 version.
				}
#endif

				blocks[i] = new TextBlock(ref dialogArea, compiledText, ref textArea, Assets.TextBG, Assets.TextBGBorderWidth, UI.DefaultDialogColor, Color.White, TipColour);
				blocks[i].GenerateCache(gfx);
			}
			int num = blocks.Length;
			do
			{
				int num2 = Main.Rand.Next(num);
				num--;
				TextBlock textBlock = blocks[num2];
				blocks[num2] = blocks[num];
				blocks[num] = textBlock;
			}
			while (num > 1);
		}

		public static TextSequenceBlock CreateTips()
		{
			int startIndex = Main.Rand.Next(blocks.Length);
			return new TextSequenceBlock(startIndex, DisplayTime);
		}

		private TextSequenceBlock(int startIndex, int displayInterval)
		{
			current = startIndex;
			cooldown = displayInterval;
			this.displayInterval = displayInterval;
		}

		public void Update()
		{
			cooldown--;
			if (cooldown < 0)
			{
				current++;
				if (current >= blocks.Length)
				{
					current -= blocks.Length;
				}
				cooldown = displayInterval;
			}
		}

		public void Draw()
		{
			blocks[current].Draw();
		}
	}
}
