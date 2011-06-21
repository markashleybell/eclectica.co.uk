function ValidateForm(f)
{
	var valid = true;
	var msg = 'The following errors occurred with your comment:\t\n\n';
	var efields = new Array();

	if(f.AuthorName.value == '') { valid = false; msg += ' - You must enter your name\t\n'; efields[efields.length] = f.AuthorName; }
	
	if(f.Email.value == '') 
	{ 
		valid = false;
		msg += ' - You must supply an email address\t\n'; 
		efields[efields.length] = f.Email; 
	}
	else
	{
		var re = /^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$/gi;
		
		if(!f.Email.value.match(re))
		{
			valid = false;
			msg += ' - The Email Address you entered was not valid\t\n'; 
			efields[efields.length] = f.Email; 
		}
	}
	
	if(f.Comment.value == '') { valid = false; msg += ' - You must enter a comment\t\n'; efields[efields.length] = f.Comment; }
	
	if(f.xcd8290.value == '') { valid = false; msg += ' - You must tell me how many monkeys are in a bag of five monkeys\t\n'; efields[efields.length] = f.xcd8290; }

	if(valid)
	{
		for(var x=0; x<f.elements.length; x++)
			f.elements[x].style.backgroundColor = '';
			
		return true;
	}
	else
	{
		alert(msg);

		for(var x=0; x<f.elements.length; x++)
			f.elements[x].style.backgroundColor = '';

		for(var x=0; x<efields.length; x++)
			efields[x].style.backgroundColor = '#f3cbcb';

		return false;
	}
}