#pragma strict

var player;

function Start ()
{
	player = GameObject.Find("Player").GetComponent("Player");
}

function Update ()
{
	
}

function Eval (str : String)
{
	eval(str);
}